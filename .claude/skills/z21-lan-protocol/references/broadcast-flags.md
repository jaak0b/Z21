# Broadcast flags (manual §2.16, §2.17)

`LAN_SET_BROADCASTFLAGS` — Header `0x50`, Data = 32-bit flags (little-endian), OR-combined.
**No reply.** Flags are stored **per (IP + source port)** and **must be re-sent on every log-on**
(i.e. after any reconnect). To confirm what the Z21 actually stored, read back with
`LAN_GET_BROADCASTFLAGS` — Header `0x51`, no data → reply `0x51` + 32-bit flags (LE).

## Flag values

| Flag | Meaning | Since |
|---|---|---|
| `0x00000001` | Driving & switching broadcasts: power off/on (§2.7/2.8), programming mode (2.9), short circuit (2.10), stopped (2.14), `LAN_X_LOCO_INFO` (4.4 — **only for subscribed loco addresses**), `LAN_X_TURNOUT_INFO` (5.3) | base |
| `0x00000002` | R-Bus feedback changes → `LAN_RMBUS_DATACHANGED` (7.1) | base |
| `0x00000004` | RailCom changes of **subscribed** locos → `LAN_RAILCOM_DATACHANGED` (8.1) | base |
| `0x00000100` | Z21 system-state changes → `LAN_SYSTEMSTATE_DATACHANGED` (2.18) | base |
| `0x00000010` | Fast-clock model-time → `LAN_FAST_CLOCK_DATA` (12.2) | FW 1.43 |
| `0x00010000` | `LAN_X_LOCO_INFO` for **all** locos without subscribing. PC-control only, not handhelds. FW 1.20–1.23: all locos; **FW ≥1.24: only *changed* locos** | FW 1.20 |
| `0x00040000` | RailCom for all locos without subscribing → `LAN_RAILCOM_DATACHANGED` (8.1). PC-control only | FW 1.29 |
| `0x00080000` | CAN-bus occupancy detectors → `LAN_CAN_DETECTOR` (10.1) | FW 1.30 |
| `0x00020000` | CAN-bus booster status → `LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD` (10.2.3) | FW 1.41 |
| `0x01000000` | Forward LocoNet bus messages to the client (excluding locos & turnouts) | FW 1.20 |
| `0x02000000` | Loco-specific LocoNet messages: `OPC_LOCO_SPD`, `OPC_LOCO_DIRF`, `OPC_LOCO_SND`, `OPC_LOCO_F912`, `OPC_EXP_CMD` | FW 1.20 |
| `0x04000000` | Turnout-specific LocoNet messages: `OPC_SW_REQ`, `OPC_SW_REP`, `OPC_SW_ACK`, `OPC_SW_STATE` | FW 1.20 |
| `0x08000000` | LocoNet occupancy detectors → `LAN_LOCONET_DETECTOR` (9.5) | FW 1.22 |

Undefined bits are reserved — leave them 0.

## Practical notes and gotchas

- The manual flags `0x00010000`, `0x00040000`, `0x02000000`, `0x04000000` as **bandwidth-heavy**
  ("PC-control only"); on a busy layout they generate a lot of traffic.
- `0x1` alone delivers `LAN_X_LOCO_INFO` **only for loco addresses the client subscribed to** via
  `LAN_X_GET_LOCO_INFO` (max 16/client). `0x10000` is the "all locos, no subscription" supplement —
  but its delivery is **single-recipient** on real hardware (last claimant wins), which is the heart
  of the loco-monitoring problem; see the z21-hardware-behavior skill before relying on it.
- Setting flags is fire-and-forget over UDP (no reply, no retransmit). If broadcasts aren't arriving,
  read flags back with `LAN_GET_BROADCASTFLAGS` to prove the Z21 stored what you sent.
- A library default value of `0x00010001` (drive + all-loco-info) omits R-Bus (`0x2`) and system
  state (`0x100`); consumers that want feedback/telemetry must add them.
