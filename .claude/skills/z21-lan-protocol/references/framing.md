# Framing, transport, and client lifecycle (manual §1, §2.2)

## Datagram structure (§1.2.1)

```
| DataLen (2 bytes, little-endian) | Header (2 bytes, little-endian) | Data (n bytes) |
```

- **`DataLen` is the whole-dataset length**, including the 2-byte DataLen field and 2-byte Header:
  `DataLen = 2 + 2 + n`. Off-by-one here silently corrupts parsing — a too-small DataLen makes the
  reader mis-frame the next dataset. `Z21FrameBuilder.BuildLan` must emit `DataLen = payload + 4`
  (e.g. `LAN_SET_BROADCASTFLAGS` is `08 00 50 00 ff ff ff ff`).
- **Little-endian** for all multi-byte fields unless a command explicitly says otherwise. The known
  big-endian exceptions are the loco/turnout *mode* commands (§3) which carry big-endian addresses.

## Combining datasets in one UDP packet (§1.3)

Several independent datasets may be concatenated in one UDP payload and are equivalent to sending
them separately. A receiver MUST walk the buffer by each dataset's `DataLen`. `Z21FrameReader` does
this: it appends bytes to a buffer, then repeatedly reads `DataLen`, emits a complete frame, and
keeps any partial trailing frame for the next packet (correct for both message/UDP and stream/serial
transports). An out-of-range `DataLen` (0 or > 1472) is treated as corruption and the buffer is
cleared to resync.

## X-Bus tunneling (§1.2.2) and LocoNet tunneling (§1.2.3)

- Header `0x40` (`LAN_X_*`) carries X-Bus-style messages. Layout: `... 40 00 <X-header> [DB0 DB1 …]
  <XOR>`. The **X-header** (and frequently a **DB0** sub-byte) selects the command. The trailing
  **XOR** byte is the running XOR of all X-bytes (X-header + DBs). `Z21FrameBuilder.BuildXBus`
  computes it. These are LAN-level messages — unrelated to the physical X-Bus.
- Header `0xA2` (`LAN_LOCONET_FROM_LAN`) lets a LAN client inject a LocoNet message. See loconet.md.

## Communication model (§1.1)

- Fully **asynchronous**: broadcast messages can arrive between a request and its reply, so never
  assume the next datagram is the answer to your last request — match by header/content.
- **No application-level acks** for most "set" commands, and UDP itself has no delivery guarantee
  (routers may drop packets). A command with "Antwort: keine" (no reply) cannot be confirmed except
  by reading state back (e.g. `LAN_GET_BROADCASTFLAGS` after `LAN_SET_BROADCASTFLAGS`).

## Client identity, lifecycle, ports

- A client is identified by **IP + source UDP port**. **Log-on is implicit** with the client's first
  datagram (§2.2) — there is no separate login handshake.
- **Inactivity timeout ~60 s** (§1.1): a client that sends nothing for a minute is dropped from the
  active list. Keep-alive with any command (this library re-queries firmware on a timer).
- **`LAN_LOGOFF`** (Header `0x30`, no data, no reply) deregisters the client immediately. Use the
  **same source port** as log-on. Sending it on disconnect frees the Z21's client slot at once
  instead of waiting out the ~60 s timeout.
- Standard command-station UDP port is **21105**. Bind considerations (some broadcasts are addressed
  to the station port, not the client's source port) are covered in the z21-hardware-behavior skill;
  the transport exposes `UdpTransportOptions.LocalPort` for this.

## Reference: serial / version / hw info (§2.1, §2.20, §2.21)

- `LAN_GET_SERIAL_NUMBER` — Header `0x10`, no data → reply `0x10` + 32-bit serial (LE).
- `LAN_GET_HWINFO` — Header `0x1A`, no data → reply `0x1A` + HwType 32-bit + FW version 32-bit (LE).
- `LAN_GET_CODE` — Header `0x18`, no data → reply `0x18` + 1-byte lock code.
