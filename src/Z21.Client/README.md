# Z21

A small, event-driven .NET client for the ROCO/Fleischmann **Z21** digital command
station. It speaks the Z21 LAN protocol (V1.13) over UDP, so you can drive locomotives,
throw turnouts, switch track power, read CVs and query system state — and get typed
events back whenever something changes on the layout.

```csharp
station.LocoInfoReceived += (_, loco) =>
    Console.WriteLine($"loco {loco.LocoAddress} now at speed {loco.LocoSpeed}");

await station.ConnectAsync();
await station.TrackPowerOnAsync();
await station.DriveAsync(3, DccSpeedMode.Steps128, DrivingDirection.Forward, 40);
```

You get an `IZ21CommandStation` from the DI container. To wire it up, add one of the
companion packages: **Z21.DependencyInjection** (Microsoft.Extensions.DependencyInjection)
or **Z21.Autofac** (Autofac). Both expose a single `AddZ21(...)` call.

The full command/response support matrix and protocol notes live on the
[project page](https://github.com/jaak0b/Z21). Licensed under GPL-3.0.
