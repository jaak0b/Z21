# System, status, power, firmware, settings (manual §2, §3)

Most of these are `LAN_X_*` (Header `0x40`) selected by X-header + DB0, except the system-state and
settings commands which are plain LAN headers.

## Track power and global stop (§2.5–2.14)

| Command | Direction | Bytes (X-header / DB0) |
|---|---|---|
| `LAN_X_SET_TRACK_POWER_OFF` | →Z21 | X `0x21`, DB0 `0x80`, XOR `0xA1` |
| `LAN_X_SET_TRACK_POWER_ON` | →Z21 | X `0x21`, DB0 `0x81`, XOR `0xA0` |
| `LAN_X_BC_TRACK_POWER_OFF` | Z21→ | X `0x61`, DB0 `0x00` |
| `LAN_X_BC_TRACK_POWER_ON` | Z21→ | X `0x61`, DB0 `0x01` |
| `LAN_X_BC_PROGRAMMING_MODE` | Z21→ | X `0x61`, DB0 `0x02` (entered CV programming) |
| `LAN_X_BC_TRACK_SHORT_CIRCUIT` | Z21→ | X `0x61`, DB0 `0x08` |
| `LAN_X_UNKNOWN_COMMAND` | Z21→ | X `0x61`, DB0 `0x82` (reply to an invalid request) |
| `LAN_X_SET_STOP` (emergency stop all) | →Z21 | X `0x80`, XOR `0x80` |
| `LAN_X_BC_STOPPED` | Z21→ | X `0x81`, DB0 `0x00` |

The Z21→ power/stop broadcasts require broadcast flag `0x1`.

## Status and versions (§2.3, §2.4, §2.12, §2.15)

- `LAN_X_GET_VERSION` — →Z21 X `0x21`, DB0 `0x24`. Reply gives X-Bus version + command-station id.
- `LAN_X_GET_STATUS` — →Z21 X `0x21`, DB0 `0x24` family; reply `LAN_X_STATUS_CHANGED`.
- `LAN_X_STATUS_CHANGED` — Z21→ Header `0x40`, X `0x62`, DB0 `0x22`, DB1 = `CentralState` bitfield.
- `LAN_X_GET_FIRMWARE_VERSION` — →Z21 X `0xF1`, DB0 `0x0A`, XOR `0xFB`. Reply X `0xF3`, DB0 `0x0A`,
  DB1 = major (BCD), DB2 = minor (BCD). Example reply `09 00 40 00 F3 0A 01 43 BB` = FW **1.43**.
  This is the library's default keep-alive ping.

## System state (§2.18, §2.19)

- `LAN_SYSTEMSTATE_GETDATA` — →Z21 Header `0x85`, no data. Reply is `LAN_SYSTEMSTATE_DATACHANGED`.
- `LAN_SYSTEMSTATE_DATACHANGED` — Z21→ Header `0x84`, `DataLen=0x14` (20 bytes total), 16-byte
  `SystemState`: main current, prog current, filtered main current, temperature, supply voltage, VCC
  voltage, `CentralState`, `CentralStateEx`, … (all little-endian 16-bit fields + state bytes).
  Sent on change when flag `0x100` is set, or once in reply to GETDATA. In practice the Z21 emits it
  roughly once per second because the analog fields jitter — a useful "is the broadcast pipe alive"
  heartbeat, though the spec does not promise a fixed rate.

## Loco / turnout output mode (§3) — note the BIG-ENDIAN addresses

- `LAN_GET_LOCOMODE` — Header `0x60`, Data = loco address 16-bit **big-endian**. Reply `0x60` +
  address (BE) + 1-byte mode.
- `LAN_SET_LOCOMODE` — Header `0x61`, address (BE) + mode (`0`=DCC, `1`=MM). Persistent. No reply.
- `LAN_GET_TURNOUTMODE` — Header `0x70`, accessory decoder address (BE). Reply `0x70` + address (BE)
  + mode.
- `LAN_SET_TURNOUTMODE` — Header `0x71`, address (BE) + mode. Persistent. No reply.

These four LAN headers (`0x60/0x61/0x70/0x71`) are easy to confuse with the X-header `0x61`
power-broadcast and with turnout commands — disambiguate by byte position (LAN header is bytes 2-3).
