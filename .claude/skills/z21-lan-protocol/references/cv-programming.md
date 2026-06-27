# CV reading & writing (manual §6)

Two worlds: **service mode** (programming track, no loco address) and **POM / main-track** (operations
mode, addressed to a loco/accessory; needs RailCom for read-back). All `LAN_X_*` (Header `0x40`).
CV addresses on the wire are **0-based** (CV1 is sent as `0x0000`); convert at the boundary.

## Service mode (programming track)

| Command | X-header / DB0 | Notes |
|---|---|---|
| `LAN_X_CV_READ` (§6.1) | `0x23`, DB0 `0x11`, CV_MSB, CV_LSB, XOR | Read a CV in service mode. Puts the Z21 into programming mode (emits `LAN_X_BC_PROGRAMMING_MODE` with flag `0x1`) |
| `LAN_X_CV_WRITE` (§6.2) | `0x24`, DB0 `0x12`, CV_MSB, CV_LSB, value, XOR | Write a CV in service mode |
| `LAN_X_CV_RESULT` (§6.5) | Z21→ `0x64`, DB0 `0x14`, CV_MSB, CV_LSB, value | The read/write result. **Carries only the CV address, not a loco address** — so don't run concurrent CV ops on one station, you can't correlate them |
| `LAN_X_CV_NACK` (§6.4) | Z21→ `0x61`, DB0 `0x13` | No decoder acknowledgement (also: a byte-wise read can simply take a long time) |
| `LAN_X_CV_NACK_SC` (§6.3) | Z21→ `0x61`, DB0 `0x12` | Short circuit during programming |

## POM (operations mode, main track) — §6.6–6.11

| Command | X-header / DB0 | Notes |
|---|---|---|
| `LAN_X_CV_POM_WRITE_BYTE` (§6.6) | `0xE6`, DB0 `0x30`, addr, then option `0xEC`+CV+value | POM byte write; **no acknowledgement** — verify by reading back |
| `LAN_X_CV_POM_WRITE_BIT` (§6.7) | `0xE6`, DB0 `0x30`, option `0xE8` | POM bit write. Build the bit-data byte carefully (the high nibble matters) |
| `LAN_X_CV_POM_READ_BYTE` (§6.8) | `0xE6`, DB0 `0x30`, option `0xE4` | Needs RailCom; result via `LAN_X_CV_RESULT` |
| `LAN_X_CV_POM_ACCESSORY_*` (§6.9–6.11) | `0xE6`, DB0 `0x31` | Same for accessory decoders (FW ≥ 1.22) |

The POM option nibble (`0xEC` write byte / `0xE8` write bit / `0xE4` read byte) is OR-ed with the top
two bits of the (0-based) CV address — get that packing right or the Z21 silently targets the wrong CV.

## Legacy / direct (§6.12–6.14)

`LAN_X_MM_WRITE_BYTE` (Märklin-Motorola), `LAN_X_DCC_READ_REGISTER`, `LAN_X_DCC_WRITE_REGISTER` —
rarely needed; see the manual for the exact bytes.

Because POM reads and CV results give no loco correlation, the library wraps them in retrying,
deadline-bounded "safe" methods (`ReadPomCvAsync`/`WritePomCvAsync`/…) that serialize CV operations.
Don't issue overlapping CV operations on one station.
