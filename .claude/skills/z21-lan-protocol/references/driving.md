# Driving locomotives (manual §4)

All `LAN_X_*` (Header `0x40`), selected by X-header. Loco addresses are 14-bit: the two address bytes
are `AddrH = (addr >> 8) | 0xC0` for addr ≥ 128 (the top two bits flag long addresses), `AddrL =
addr & 0xFF`. The library encapsulates this in `IAddressCodec` and speed in `ILocoSpeedCodec` — reuse
them rather than re-deriving the bit math.

The Z21 tracks at most **16 loco addresses per client** for subscription (§4); a 17th evicts the
oldest (FIFO).

## Commands

| Command | X-header / DB0 | Notes |
|---|---|---|
| `LAN_X_GET_LOCO_INFO` (§4.1) | `0xE3`, DB0 `0xF0`, then AddrH, AddrL, XOR | Polls a loco **and subscribes** the client to its `LAN_X_LOCO_INFO` (with flag `0x1`). e.g. addr 3 → `09 00 40 00 E3 F0 00 03 10` |
| `LAN_X_SET_LOCO_DRIVE` (§4.2) | `0xE4`, DB0 `0x1R` (R = speed-step format: `0x10`=14, `0x12`=28, `0x13`=128), AddrH, AddrL, RVVVVVVV | Top bit of the speed byte = direction (`0x80` = forward); lower 7 bits = speed in the chosen step range |
| `LAN_X_SET_LOCO_FUNCTION` (§4.3.1) | `0xE4`, DB0 `0xF8`, AddrH, AddrL, function byte | Function byte: bits 5-6 = toggle type (00 off, 01 on, 10 toggle), bits 0-4 = function index |
| `LAN_X_SET_LOCO_FUNCTION_GROUP` (§4.3.2) | `0xE4`, DB0 group id | Sets a whole function group at once |
| `LAN_X_SET_LOCO_BINARY_STATE` (§4.3.3) | `0xE4`, DB0 `0x5F` | DCC binary state |
| `LAN_X_SET_LOCO_E_STOP` (§4.5) | `0x92`, AddrH, AddrL, XOR | Emergency-stop one loco |
| `LAN_X_PURGE_LOCO` (§4.6) | `0xE3`, DB0 `0x44`, AddrH, AddrL, XOR | Drop the loco from the Z21's active/refresh list |

## LAN_X_LOCO_INFO (§4.4) — the response/broadcast

Z21→ Header `0x40`, X-header `0xEF`, then `DataLen = 7 + n` (`7 ≤ n ≤ 14`; FW ≥ 1.42 uses
`DataLen ≥ 15` to carry F29–F31). Layout after `0xEF`:

```
AddrH AddrL  DB2  DB3  DB4  DB5  …  XOR
```

- `AddrH AddrL` — loco address (mask off the long-address flag bits in AddrH).
- `DB2` — bit0-2: speed-step mode (`000`=14, `010`=28, `100`=128); bit3: loco is "busy"/controlled by
  another device; (other bits double-traction/smart-search).
- `DB3` — bit7: direction (1=forward); bits0-6: speed in the step range.
- `DB4+` — function state bitmaps (F0…F31 across the following bytes).

Example: `0F 00 40 00 EF 00 03 04 80 00 00 00 00 00 7A` = loco 3, 128-step, forward, speed 0.

When is it sent unsolicited? With flag `0x1` **and** the loco subscribed (`LAN_X_GET_LOCO_INFO`), or
with flag `0x10000` for all (changed, FW ≥1.24) locos. **Critical real-hardware caveat:** the
`0x10000` "all locos" delivery is **single-recipient** — the Z21 sends a loco's info to only the
client that last claimed it (registered `0x10000` or drove it). A passive monitor cannot share a
loco's `LAN_X_LOCO_INFO` with an active controller. See the **z21-hardware-behavior** skill for the
evidence and the LocoNet-based workaround.
