# Feedback — R-Bus (manual §7)

R-Bus occupancy detectors, gated by broadcast flag `0x00000002`.

| Command | Header | Notes |
|---|---|---|
| `LAN_RMBUS_GETDATA` (§7.2) | `0x81`, 1-byte group index | Request current state of a feedback group |
| `LAN_RMBUS_DATACHANGED` (§7.1) | Z21→ `0x80`, group index + 10 status bytes | Sent on change (flag `0x2`) and as the reply to GETDATA |
| `LAN_RMBUS_PROGRAMMODULE` (§7.3) | `0x82`, 1-byte address | Assign an R-Bus module address |

`LAN_RMBUS_DATACHANGED` layout: Header `0x80`, then `groupIndex` (0 or 1), then **10 bytes** of
occupancy bits. Each byte is one feedback module (8 contacts); group 0 covers modules 1–10, group 1
covers 11–20. Bit set = contact occupied.

Example Z21→ `0F 00 80 00 00 83 06 05 00 00 00 00 00 00 00` = group 0, modules with bits in the
`0x83 0x06 0x05 …` pattern occupied.

To seed current state at connect time, send `LAN_RMBUS_GETDATA` for group 0 and group 1; the replies
arrive as `LAN_RMBUS_DATACHANGED`. (RailCom feedback is separate — see extended-features.md.)
