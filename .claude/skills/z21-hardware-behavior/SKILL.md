---
name: z21-hardware-behavior
description: >-
  Hard-won, hardware-verified knowledge of how a real ROCO/Fleischmann Z21 command station actually
  behaves on the wire — the surprising, undocumented-or-buried quirks, plus how to diagnose them.
  Use this whenever debugging "why isn't the Z21 sending / delivering X", missing or intermittent
  loco-info, broadcasts that arrive for one client but not another, reconnect or multi-client
  weirdness, broadcast-flag confusion, UDP local-port binding, or whenever capturing/interpreting
  Z21 UDP traffic (Wireshark/tshark). Reach for it before theorizing about Z21 behavior from the
  spec alone — the spec describes intent, this skill records what the hardware really does. Pairs
  with the z21-lan-protocol skill (wire formats).
---

# Z21 hardware behavior & diagnostics

The Z21 LAN spec describes *intent*; real firmware (verified on FW 1.43) has behaviors that aren't
obvious from the document and have repeatedly cost hours. This skill records those behaviors and the
techniques that reveal them. For wire formats and command bytes, use the **z21-lan-protocol** skill.

## The headline quirks (details in references/loco-info-delivery.md)

1. **`LAN_X_LOCO_INFO` (flag `0x10000`) is single-recipient — last claimant wins.** The Z21 sends a
   given loco's info to the *one* client that most recently "claimed" it, where claiming =
   registering `0x10000` **or** driving the loco (`LAN_X_SET_LOCO_DRIVE`). A second `0x10000` client
   does **not** also receive it. Opening a fresh client steals a loco from a controller; the
   controller steals it back the instant it drives. A passive monitor and an active controller can
   never both receive a loco's `LAN_X_LOCO_INFO`. This is firmware behavior, not a client bug.

2. **Some broadcasts are addressed to the command-station port (21105), not the client's source
   port.** A client on an OS-assigned ephemeral local port receives system-state and R-Bus fine but
   silently misses `LAN_X_LOCO_INFO`. Bind the local UDP socket to the station's port (what the
   pre-v7 library did) to receive them. In this library that's `UdpTransportOptions.LocalPort`
   (null → bind the station port; 0 → ephemeral).

3. **Broadcast flags are per (IP + source port) and reset on every log-on; the SET has no reply.**
   Re-send flags after any reconnect, and confirm with `LAN_GET_BROADCASTFLAGS` — a dropped/raced
   SET packet (UDP, no ack) leaves you connected but receiving nothing, with no error.

4. **Subscription (`LAN_X_GET_LOCO_INFO`) is per-client but capped at 16 locos** (FIFO). It coexists
   with a controller (unlike `0x10000`) but can't cover an unlimited fleet.

5. **LocoNet loco messages (`0x02000000`) are a parallel feed that survives a controller.** They
   reach a passive client even while another client owns the loco's `LAN_X_LOCO_INFO` — the only
   passive way to observe *all* locos alongside a controller — but they're keyed by LocoNet slot, so
   they need slot↔address tracking (see the z21-lan-protocol skill, `references/loconet.md`).

6. **Client slots age out after ~60 s of silence; disconnects without `LAN_LOGOFF` leave stale
   entries.** Spamming short-lived clients can exhaust the Z21's client/registration table (it then
   stores flags but delivers no broadcasts). Send `LAN_LOGOFF` on disconnect; keep-alive while
   connected. (This was a red herring for the loco-info issue but is a real operational gotcha.)

## Smaller gotchas

- **`LAN_SYSTEMSTATE_DATACHANGED` arrives ~1/sec** in practice (analog fields jitter) — a handy
  "is the broadcast pipe alive" heartbeat, though the spec doesn't guarantee a rate. Its presence
  while loco-info is absent is the classic "flags OK, but this loco isn't being delivered to me" sign.
- **Binding a fixed local port (21105) triggers a Windows Firewall prompt**; an ephemeral port
  doesn't. A sudden firewall prompt is a useful confirmation that a local-port change took effect.
- **`0x10000` is "changed locos only" on FW ≥ 1.24** — a loco sitting at constant speed produces no
  `LAN_X_LOCO_INFO` at all. "No loco events" can simply mean "nothing is changing"; wiggle the
  throttle when testing.
- **A controller like iTrain uses `0x10000` too** (not `GET_LOCO_INFO`); it wins delivery purely by
  being the active driver. It is not doing anything special you can copy via flags alone.

## Diagnosing Z21 behavior

When a behavior is in doubt, **measure on the wire** — don't reason from the spec. The full
playbook (tshark recipes, instrumented-build technique, headless probes, the wrong-turns to avoid)
is in **references/diagnostics.md**. The single most decisive move that has cracked these issues:
**capture with Wireshark/tshark and compare the destination UDP port of `LAN_X_LOCO_INFO` packets
against each client's bound port** — it tells you *who* the Z21 is actually sending to, which no
amount of log-reading on one client can.
