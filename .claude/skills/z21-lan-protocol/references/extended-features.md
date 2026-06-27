# Extended features: RailCom, CAN, zLink, fast clock (manual §8, §10, §11, §12)

These are specialist areas. Headers below are from the spec; open the cited section for the full
struct layouts before implementing — most of these carry fixed-size binary state structs.

## RailCom (§8)

Actual decoder feedback (address, receive/error counts, options, QoS) via RailCom detectors.

| Command | Header | Notes |
|---|---|---|
| `LAN_RAILCOM_DATACHANGED` (§8.1) | Z21→ `0x88`, `DataLen=0x11` | RailCom data record; keyed by **loco address** |
| `LAN_RAILCOM_GETDATA` (§8.2, FW ≥1.29) | `0x89`, type 8-bit + loco address 16-bit (LE) | Request RailCom data |

Flags: `0x00000004` = RailCom for **subscribed** locos; `0x00040000` = RailCom for **all** locos
(no subscription, PC-control only, FW ≥1.29). Because RailCom is address-keyed, the all-locos RailCom
feed is a possible alternative to LocoNet for passive monitoring — **if** the layout has RailCom
decoders and a RailCom detector (otherwise these never arrive), and subject to the same delivery
caveats as other all-loco feeds. Untested here.

## CAN bus (§10)

| Command | Header | Notes |
|---|---|---|
| `LAN_CAN_DETECTOR` (§10.1) | CAN occupancy detectors | flag `0x00080000`; request/reply per §10.1 |
| `LAN_CAN_BOOSTER_GET_DESCRIPTION` (§10.2) | `0xC8`, NId 16-bit | reply `0xC8` + NId + `Name[16]` |
| `LAN_CAN_BOOSTER_SET_DESCRIPTION` | `0xC9`, NId + `Name[16]` | no reply; forbidden chars `"` and `\` |
| `LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD` (§10.2.3) | Z21→ `0xCA`, `DataLen=0x0E`, 10-byte state | flag `0x00020000` |
| `LAN_CAN_BOOSTER_SET_TRACKPOWER` | `0xCB`, NId + power 8-bit | enable/disable booster track outputs |

## zLink (§11) — booster / decoder / adapter over the zLink

| Command | Header | Notes |
|---|---|---|
| zLink HW info | `0xE8`, Data[0]=`0x06` | per-LINK hardware info (58-byte struct) |
| Booster name get/set | `0xB8` / `0xB9` | `Name[32]`, ISO-8859-1; `Name[0]==0xFF` = never set |
| Booster system state | `0xBA` (push), `0xBB` (request) | 24-byte `BoosterSystemState` |
| Booster set port state | `0xB2`, port + state | |
| Decoder name/state | `0xD8`/`0xD9` name, `0xDA` push, `0xDB` request | switch/signal decoder state structs |

## Fast clock / model time (§12, FW ≥1.43)

| Command | Header | Notes |
|---|---|---|
| `LAN_FAST_CLOCK_CONTROL` (§12.1) | `0xCC`, Data `0x21 0x2A 0x0B` (read) / `0x21 0x2C 0x0D` (start) / `0x21 0x2D 0x0C` (stop) / set form | start/stop persist; replies go to subscribers (flag `0x10`) |
| `LAN_FAST_CLOCK_DATA` (§12.2) | Z21→ `0xCD`, `DataLen=0x0C`, 8-byte `FastClockTime` | the current model time |
| `LAN_GET_FAST_CLOCK_SETTINGS` (§12.3) | `0xCE`, Data `0x04` | reply `0xCE` + `FcSettings` |
| `LAN_SET_FAST_CLOCK_SETTINGS` (§12.4) | `0xCF`, settings | persist model-clock config |

## Library note

None of these have a neutral capability interface in this library's domain API — they're reachable
only through the Z21 escape hatch (`IZ21CommandStation.Commands` + `SendCommandsAsync`, and the Z21
response-handler events). If you add first-class support, follow the per-command/per-handler file
pattern and update the README support matrix.
