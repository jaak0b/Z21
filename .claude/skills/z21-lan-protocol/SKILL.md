---
name: z21-lan-protocol
description: >-
  Reference for the ROCO/Fleischmann Z21 LAN protocol (UDP) as implemented by this library
  (repo E:\Development\Z21, NuGet `Z21`). Use this whenever working on Z21 protocol code — adding
  or changing a command/response, decoding a datagram, touching broadcast flags, framing, the UDP
  transport, or loco/turnout/CV/feedback/RailCom/LocoNet/CAN handling, or interpreting a
  Wireshark/tshark capture of Z21 traffic. The full spec is large and in German; this skill is the
  navigable English map. Reach for it even when the task only mentions "the Z21", a `LAN_X_*`/`LAN_*`
  command, an X-header byte, a datagram hex dump, or "why isn't the Z21 sending X" — don't
  reconstruct the wire format from memory, check the relevant reference file.
---

# Z21 LAN protocol

The authoritative spec is **`src/api/z21-lan-protokoll.md`** (German, ~1600 lines, ROCO Z21 LAN
Protokoll Spezifikation V1.13, covers firmware up to 1.43). This skill is a navigable summary plus
the gotchas that bite in practice. When a reference file is not enough, open the manual at the cited
section number.

## How the library is laid out (so you know where code goes)

- Commands: `src/Z21.Client/Core/Command/<area>/` — one file per command, built via `IZ21FrameBuilder`.
- Response handlers: `src/Z21.Client/Core/ResponseHandler/<area>/` — `CanHandle`/`Handle`, raise events.
- Framing: `Z21FrameReader` (splits the stream by `DataLen`), `Z21FrameBuilder` (`BuildLan`/`BuildXBus`).
- Transport: `CommandStation.Transport.Udp/UdpTransport`.
- A new command is one new file (+ optional factory method); a new handler/parser is auto-discovered
  by DI reflection — no registration edits. Keep the README support matrix in sync.

## Framing essentials (read references/framing.md before touching the wire)

Every datagram is one or more **datasets** back-to-back in a single UDP packet:

```
| DataLen (2B, LE) | Header (2B, LE) | Data (n B) |
```

- `DataLen` = the WHOLE dataset length incl. the 2-byte DataLen and 2-byte Header (`2 + 2 + n`).
- All multi-byte fields are **little-endian** unless a command says otherwise (loco/turnout *mode*
  commands use big-endian addresses — easy to get wrong).
- Multiple datasets may be combined in one UDP packet; a receiver must walk them by `DataLen`.
- Header `0x40` (`LAN_X_*`) tunnels X-Bus-style messages; the **X-header** (first Data byte) plus
  often a **DB0** sub-byte select the actual command, and an **XOR** check byte trails the X-data.

## Header → area map (which reference file to open)

All reference files live in `references/`. Open the one for the area you're working in:

| LAN Header (bytes 2-3) | Area | Reference file |
|---|---|---|
| `0x10` serial, `0x1A` hwinfo, `0x18` code, `0x85`/`0x84` system state, `0x30` logoff, `0x40` X-power/status/version/stop/firmware | System, status, power, lifecycle | `references/system-status-power.md` |
| `0x50` set / `0x51` get broadcast flags | Broadcast flags | `references/broadcast-flags.md` |
| `0x60`/`0x61` loco mode, `0x40` X-header `0xE3 0xF0` get-loco-info / `0xE4` drive+functions / `0xEF` loco-info | Driving | `references/driving.md` |
| `0x70`/`0x71` turnout mode, `0x40` X-header `0x43` turnout / `0x53` set-turnout / `0x44` ext-accessory | Switching | `references/switching.md` |
| `0x40` X-header `0x23` CV read / `0x24` CV write / `0x64` result / `0xE6` POM / `0x61 DB0 0x12/0x13` NACK | CV programming | `references/cv-programming.md` |
| `0x80` R-Bus changed / `0x81` get / `0x82` programmodule | Feedback (R-Bus) | `references/feedback-rbus.md` |
| `0x88` RailCom changed / `0x89` get | RailCom | `references/extended-features.md` |
| `0xA0`-`0xA4` LocoNet (RX/TX/from-LAN/dispatch/detector) | LocoNet | `references/loconet.md` |
| `0xC4`/`0xC8`-`0xCB` CAN, `0xCC`-`0xCF` fast clock, `0xE8`/`0xB*`/`0xD8`-`0xDB` zLink | CAN, fast clock, zLink | `references/extended-features.md` |

Plus `references/framing.md` for the datagram structure, combining, X-Bus/LocoNet tunneling, and the
client lifecycle — read it before touching the wire format or the transport.

Note the collision trap: `0x60/0x61/0x70/0x71` as **LAN headers** (bytes 2-3) are loco/turnout
*mode*, but `0x61` as an **X-header** (byte 4, inside a `0x40` frame) is a track-power/status
broadcast. Always disambiguate by position.

## Cross-cutting behaviors that surprise people

These are spelled out where relevant in the reference files, but the big ones:

- **Broadcast flags are per (IP + source port)** and reset on every (re)connect — re-send them after
  reconnecting. `LAN_SET_BROADCASTFLAGS` has **no reply**; confirm with `LAN_GET_BROADCASTFLAGS`.
- **`LAN_X_LOCO_INFO` via flag `0x10000` is single-recipient** — the Z21 sends a loco's info to the
  one client that last "claimed" it (registered `0x10000` or drove it), not to every `0x10000` client.
  See the **z21-hardware-behavior** skill for the full story and the LocoNet alternative.
- Some broadcasts are sent to the **command-station port (21105)**, not the client's source port, so
  the local UDP socket should bind that port — see framing.md and the `UdpTransportOptions.LocalPort`.
- Client is dropped after ~60 s of silence; keep-alive with any command (the library re-queries
  firmware). `LAN_LOGOFF` (header `0x30`) frees the slot immediately on disconnect.

## When editing protocol code

Match the existing per-command/per-handler file pattern, keep multi-byte fields little-endian (watch
the big-endian exceptions), assert on exact datagram bytes in tests (the suite gates on mutation
testing), and update the support matrix in `README.md`.
