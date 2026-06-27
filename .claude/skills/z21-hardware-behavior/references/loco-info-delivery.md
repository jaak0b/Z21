# How the Z21 delivers loco info (the full story)

Verified on hardware (FW 1.43) with Wireshark. This is the model that fits every observation; the
spec does not state it plainly.

## What the wire showed

- A passive LAN client with flags `0x00010103` (drive + R-Bus + system-state + all-loco-info), bound
  to an **ephemeral** local port, received `LAN_SYSTEMSTATE_DATACHANGED` (~1/sec) and R-Bus changes,
  but **zero `LAN_X_LOCO_INFO`**, while a loco was actively driven.
- The Z21 was sending the `LAN_X_LOCO_INFO` packets — but addressed to UDP **port 21105** (or to a
  controller's source port), not to that client's ephemeral port. Binding the client's local socket
  to the station port (21105) made loco-info arrive immediately.
- With a controller (e.g. iTrain) connected and driving, every `LAN_X_LOCO_INFO` went to the
  **controller's** source port and the passive client got none — even with `0x10000` set and bound
  to 21105. Live toggling confirmed: open the passive client → it wins the loco; let the controller
  drive → the controller wins it back. Last claim wins.
- The controller used the same `0x10000` flag and did **not** subscribe via `LAN_X_GET_LOCO_INFO`
  (no `0xE3` on the wire). It won purely by being the active driver.
- With the LocoNet flags (`0x01000000 | 0x02000000`) set, the passive client received `OPC_LOCO_SPD`
  messages (`LAN_LOCONET_Z21_TX`, header `0xA1`) for the driven loco **even while the controller
  owned its `LAN_X_LOCO_INFO`** — e.g. `08 00 A1 00 A0 07 21 79` (slot 7, speed 0x21).

## The model

1. `LAN_X_LOCO_INFO` for a given loco is delivered to **one** client — the most recent to claim that
   loco. Claim = register `0x10000`, or drive the loco. The "all locos" `0x10000` flag makes you
   *eligible*, not a guaranteed parallel recipient.
2. Delivery destination is the claiming client's address; for an otherwise-unclaimed loco the Z21
   addresses the broadcast to the **station port (21105)**, so only a client bound there receives it.
3. System-state (`0x100`) and R-Bus (`0x2`) go to the client's source port and are not subject to
   the single-recipient loco behavior — which is why "everything works except loco-info" happens.
4. `0x10000` on FW ≥ 1.24 only fires for *changed* locos.

## Consequences for design

- **Standalone passive monitoring (no other controller):** bind the local port to the station port
  (`UdpTransportOptions.LocalPort = null` → 21105) and use `0x10000`. Works. This is the regression
  fix (pre-v7 bound the station port; v7 used ephemeral and broke it).
- **Monitoring while a controller (iTrain) drives:** `0x10000` cannot work (single-recipient).
  Options:
  - `LAN_X_GET_LOCO_INFO` subscription — coexists with the controller, but **max 16 locos/client**.
  - **LocoNet** (`0x02000000`) — the only passive *all-loco* feed that coexists, but slot-keyed; needs
    a slot↔address tracker built from `OPC_SL_RD_DATA`/`OPC_LOCO_ADR` (see the z21-lan-protocol skill,
    `references/loconet.md`).
  - RailCom all-locos (`0x00040000`) — address-keyed, but needs RailCom hardware and is unverified;
    may share the same single-recipient caveat.

## Wrong turns we made (so you don't repeat them)

- "The library is dropping loco frames" — **wrong.** An instrumented build proved every frame that
  *arrives* is parsed and handled; the frames simply weren't being sent to that client's port.
- "Stale client slots / missing `LAN_LOGOFF` cause the loco-info loss on reconnect" — **wrong** as the
  loco-info cause (sending `LAN_LOGOFF` is still good hygiene, just unrelated here).
- "You must subscribe per loco" — only partly: subscription is one option, but it's 16-capped, and a
  bound-to-21105 `0x10000` client gets all locos when no controller owns them.
- Comparing a Wireshark capture from one moment against a log from another — loco-info is
  change-driven and intermittent; always capture and log the **same window** simultaneously.
