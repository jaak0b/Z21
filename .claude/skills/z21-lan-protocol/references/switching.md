# Switching: turnouts & accessories (manual §5)

All `LAN_X_*` (Header `0x40`). Accessory addresses are 11-bit, big-endian split across two bytes.
(Loco/turnout *output mode* — DCC vs MM — is set separately via the LAN-header `0x70/0x71` commands;
see system-status-power.md.)

## Turnouts (§5.1–5.3)

| Command | X-header | Notes |
|---|---|---|
| `LAN_X_GET_TURNOUT_INFO` (§5.1) | `0x43`, AddrMSB, AddrLSB, XOR | Query a turnout's position |
| `LAN_X_SET_TURNOUT` (§5.2) | `0x53`, AddrMSB, AddrLSB, DB2, XOR | Drive a turnout output |
| `LAN_X_TURNOUT_INFO` (§5.3) | `0x43`, AddrMSB, AddrLSB, ZZ, XOR | Z21→ status; needs flag `0x1` |

`LAN_X_SET_TURNOUT` DB2 bits:
- bit3 (`A`) = activate (1) / deactivate (0) the output.
- bit0 (`P`) = which of the two outputs (`0`/`1`).
- bit5..4 (`Q`) = queue/execute mode: `Q=0` is the normal "switch now"; `Q=1` is the special
  immediate form (§5.2.1/§5.2.2 cover the two encodings).

`LAN_X_TURNOUT_INFO` position byte: `0`=unknown, `1`=output 0 active, `2`=output 1 active, `3`=both
(invalid). Example Z21→ `09 00 40 00 43 00 04 02 45` = turnout 4, position "output 1".

## Extended accessory (§5.4–5.6)

| Command | X-header / DB0 | Notes |
|---|---|---|
| `LAN_X_SET_EXT_ACCESSORY` (§5.4) | `0x44`, AddrMSB, AddrLSB, payload, XOR | Drive an extended accessory (e.g. multi-aspect signal) with a raw state byte |
| `LAN_X_GET_EXT_ACCESSORY_INFO` (§5.5) | `0x44` (query form) | Read back |
| `LAN_X_EXT_ACCESSORY_INFO` (§5.6) | `0x45` | Z21→ status |

For exact DB layouts and the `Q=0`/`Q=1` distinction, open the manual §5 — these are stable but
fiddly bitfields and worth copying verbatim into tests.
