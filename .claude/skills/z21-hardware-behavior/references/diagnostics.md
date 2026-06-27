# Diagnosing Z21 behavior on real hardware

The rule: when Z21 behavior is in doubt, **measure on the wire and instrument the code** — don't
reason from the spec. These techniques cracked the loco-info mystery.

## 1. Packet capture with tshark/Wireshark (the decisive tool)

`tshark`/`dumpcap` ship with Wireshark (e.g. `C:\Program Files\Wireshark\`), and Wireshark has a
built-in **Z21 dissector** that decodes commands (`LAN_X_LOCO_INFO`, `LAN_SYSTEMSTATE_DATACHANGED`,
…) and even temperature/voltage — so you rarely decode hex by hand.

Find the interface on the Z21's subnet:
```bash
"/c/Program Files/Wireshark/tshark.exe" -D                       # list interfaces (note the number)
# PowerShell: which iface/IP routes to the Z21
powershell -Command "Find-NetRoute -RemoteIPAddress 192.168.0.111 | Select-Object -First 1 InterfaceAlias, IPAddress"
```

Capture all Z21 traffic to a file:
```bash
"/c/Program Files/Wireshark/tshark.exe" -i <n> -f "udp port 21105" -a duration:45 -w cap.pcapng
```

The decisive analysis — **which UDP port does each message type go to?**
```bash
TS="/c/Program Files/Wireshark/tshark.exe"
# loco-info packets from the Z21, grouped by destination port (= which client gets them):
"$TS" -r cap.pcapng -Y 'ip.src==192.168.0.111' -T fields -e udp.dstport -e _ws.col.Info | grep -i LOCO_INFO | awk '{print $1}' | sort | uniq -c
# what each PC client SENDS (identify controller vs passive client by their commands):
"$TS" -r cap.pcapng -Y 'ip.dst==192.168.0.111' -T fields -e udp.srcport -e _ws.col.Info | sed 's/,.*//' | sort | uniq -c
# absolute timestamps, to line up against a log:
"$TS" -r cap.pcapng -t ad -Y 'ip.src==192.168.0.111 && udp.dstport==<port>' -T fields -e frame.time -e _ws.col.Info
```

Find a given process's bound UDP port to correlate it with the capture:
```bash
powershell -Command "Get-NetUDPEndpoint -OwningProcess (Get-Process -Name '<proc>').Id | Select LocalAddress,LocalPort"
```

Caveat: on switched Wi-Fi you only see traffic to/from **this** host — you won't see another
device's (e.g. a phone's) unicast to the Z21. That's fine for "what is the Z21 sending to *me*"
questions, which is usually what matters.

## 2. Instrument the library and run a real consumer against it

To see whether frames arrive and get handled, add `Debug`-level logging at the seams (this is shipped
in the library): `[RX]` per datagram in `UdpTransport`, `[FR]` per append/emitted-frame in
`Z21FrameReader`, `[DISP] NO handler matched` in `Z21ResponseHandler`, `[CONN]` for the bound
endpoint. To run a consumer app on the instrumented build without releasing:
```bash
dotnet pack src/Z21.sln -c Debug -o local-packages
# point the consumer at it: add local-packages as a NuGet source + pin the prerelease version.
# nbgv off-main yields the same version for uncommitted edits, so clear the extracted cache so new
# content is picked up:
rm -rf "C:/Users/<you>/.nuget/packages/z21/<ver>" "…/z21.autofac/<ver>" "…/commandstation.transport.udp/<ver>"
```
Then verify the *instrumented* build is the one actually running (its `[CONN]` line shows the bound
local port; `Get-NetUDPEndpoint` confirms the process/port). Stale duplicate processes binding the
same port are a classic source of "I changed it but nothing changed."

## 3. Headless probes

A tiny console that calls `AddZ21(...)`, subscribes to `station.LocoInfoReceived` **and**
`transport.OnBytesReceived` (log raw hex + flag any `40-00-EF` loco frame), connects, and runs for N
seconds is the fastest way to test a hypothesis without a UI. Resolve `BroadcastFlagsResponseHandler`
and send `LAN_GET_BROADCASTFLAGS` to confirm stored flags.

**Important probe caveat:** a *synthetic* LAN driver (your own client sending `LAN_X_SET_LOCO_DRIVE`)
does **not** reliably make the Z21 forward loco-info to other clients. For delivery tests, drive from
a real controller or the official phone app, and have a human change the speed during the capture.

## 4. Cross-check wire vs. handled

The pair that disambiguates everything:
- **Wire (tshark):** did the Z21 send the frame, and to which port?
- **Library log (`[RX]`/handler "handling …"):** did this client receive and process it?

`Z21 sent it to port X` + `client bound to Y ≠ X` = a routing/port problem, not a parsing bug.
`On the wire to my port` + `no "handling" log` = a real client/parse problem. We wasted time
concluding "the library drops frames" until this cross-check proved the frames never came to our port.

## 5. Resetting Z21 state between experiments

Stale client registrations linger ~60 s (no `LAN_LOGOFF` on hard kills). When results get flaky from
many short-lived test clients, **power-cycle the Z21** for a clean baseline, and avoid leaving zombie
processes bound to the station port.
