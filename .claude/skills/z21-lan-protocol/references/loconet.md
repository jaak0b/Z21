# LocoNet tunneling (manual §9, §1.2.3)

The Z21 bridges its LAN interface to a LocoNet bus. Raw LocoNet messages are tunneled in these LAN
datagrams; the **first Data byte after the header is the LocoNet opcode**, and the LocoNet message
carries its own trailing checksum.

| Command | Header | Direction / meaning |
|---|---|---|
| `LAN_LOCONET_Z21_RX` (§9.1) | `0xA0` | Z21→ a message the Z21 **received** from the LocoNet bus |
| `LAN_LOCONET_Z21_TX` (§9.2, FW ≥1.20) | `0xA1` | Z21→ a message the Z21 **wrote** to the LocoNet bus (e.g. mirroring a LAN drive) |
| `LAN_LOCONET_FROM_LAN` (§1.2.3) | `0xA2` | →Z21 a LAN client injects a message onto the bus |
| `LAN_LOCONET_DISPATCH_ADDR` (§9.x) | `0xA3` | dispatch a loco address (req: addr 16-bit LE; FW ≥1.22 reply adds an 8-bit result) |
| `LAN_LOCONET_DETECTOR` (§9.5) | `0xA4` | occupancy/transponding detector reports (flag `0x08000000`) |

Forwarding is gated by the broadcast flags: `0x01000000` (general LocoNet), `0x02000000`
(loco-specific: SPD/DIRF/SND/F912/EXP), `0x04000000` (turnout-specific), `0x08000000` (detectors).

## LocoNet opcodes you care about for loco state

These ride inside `LAN_LOCONET_Z21_RX`/`_TX`:

| Opcode | Name | Payload | Use |
|---|---|---|---|
| `0xA0` | `OPC_LOCO_SPD` | `A0 <slot> <spd> <chk>` | loco speed change (by **slot**, not address) |
| `0xA1` | `OPC_LOCO_DIRF` | `A1 <slot> <dirf> <chk>` | direction + F0–F4 |
| `0xA2` | `OPC_LOCO_SND` | `A2 <slot> <snd> <chk>` | F5–F8 |
| `0xBF` | `OPC_LOCO_ADR` | request slot for an address | start of slot acquisition |
| `0xE7` | `OPC_SL_RD_DATA` | full slot read: slot, **address**, speed, dirf, snd, … | binds **slot ↔ address** |

Example tunneled speed message: `08 00 A1 00 A0 07 21 79` = `LAN_LOCONET_Z21_TX`, `OPC_LOCO_SPD`,
slot 7, speed `0x21` (33), checksum `0x79`.

## Why this matters (the loco-monitoring escape hatch)

LocoNet loco messages are a **separate delivery path** from `LAN_X_LOCO_INFO`. On real hardware they
reach a client with flag `0x02000000` **even while another controller owns the loco's
`LAN_X_LOCO_INFO`** (which is single-recipient — see the z21-hardware-behavior skill). So enabling
`0x01000000 | 0x02000000` lets a passive client observe *all* loco speed/dir/function changes
regardless of who is driving — the catch is they are **keyed by LocoNet slot**, so the client must
track the slot↔address mapping by watching `OPC_SL_RD_DATA` (and `OPC_LOCO_ADR`). That slot tracker
is the work required to turn this into address-keyed loco info. The library has **no LocoNet decoding
today**; this is the documented path if/when passive multi-loco monitoring alongside a controller is
implemented.
