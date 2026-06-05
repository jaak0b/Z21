# Getting Started

This guide takes you from an empty project to driving a locomotive and reacting to
status events.

## Prerequisites

- **.NET 8 SDK** or later.
- A **Z21** (or compatible command station) reachable on your network, or a simulator.
  The factory-default endpoint is `192.168.0.111:21105`.

## 1. Install the packages

The core library is the [`Z21`](https://www.nuget.org/packages/Z21/) package. Add one of
the integration packages for dependency injection:

```bash
dotnet add package Z21
dotnet add package Z21.DependencyInjection   # Microsoft.Extensions.DependencyInjection
# or
dotnet add package Z21.Autofac               # Autofac
```

`Z21` contains the full protocol implementation; the integration packages only add the
`AddZ21(...)` registration helper, which wires up the concrete UDP transport and
discovers all response handlers automatically.

## 2. Register and resolve the command station

# [.NET DI](#tab/net-di)

```csharp
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Z21.Core;
using Z21.DependencyInjection;

var services = new ServiceCollection();
services.AddZ21(transport =>
    transport.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.111"), 21105));

await using var provider = services.BuildServiceProvider();
var station = provider.GetRequiredService<IZ21CommandStation>();
```

# [Autofac](#tab/autofac)

```csharp
using System.Net;
using Autofac;
using Z21.Autofac;
using Z21.Core;

var builder = new ContainerBuilder();
builder.AddZ21(transport =>
    transport.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.111"), 21105));

var container = builder.Build();
var station = container.Resolve<IZ21CommandStation>();
```

***

Resolving `IZ21CommandStation` activates the inbound message dispatcher — if you never
resolve the station, incoming frames are never processed and no events fire.

## 3. Connect

`ConnectAsync` opens the UDP transport and logs on to the Z21 (broadcast flags +
firmware query); a protocol keep-alive then runs automatically.

```csharp
await station.ConnectAsync();
```

## 4. Subscribe to events and drive

```csharp
station.LocoInfoReceived += (_, loco) =>
    Console.WriteLine($"Loco {loco.LocoAddress} @ {loco.LocoSpeed} ({loco.DrivingDirection})");
station.TrackPowerChanged += (_, on) =>
    Console.WriteLine($"Track power: {(on ? "ON" : "OFF")}");

await station.TrackPowerOnAsync();
await station.DriveAsync(
    locoAddress: 13,
    speedMode: DccSpeedMode.Steps128,
    direction: DrivingDirection.Forward,
    speed: 40);
```

## 5. Disconnect

`Z21CommandStation` is `IDisposable`/`IAsyncDisposable`. Disposing (or
`await DisconnectAsync()`) stops the keep-alive and closes the transport.

```csharp
await station.DisconnectAsync();
```

## Raw commands and Z21-only features

Features without a neutral capability (LocoNet, CAN, RailCom, zLink) are reachable through
the Z21 escape hatch on `IZ21CommandStation`: build commands with `station.Commands`
(an `IZ21CommandFactory`) and send them with `SendCommandsAsync`.

```csharp
await station.SendCommandsAsync(
    station.Commands.Create<GetFirmwareVersionCommand>(),
    station.Commands.Create<GetLocoInfoCommand>((ushort)13)); // one UDP packet
```

> [!NOTE]
> `Create<TCommand>(...)` binds constructor arguments by exact type — cast integer
> literals to the parameter type (e.g. `(ushort)13`).

Browse the full API reference — start at [`IZ21CommandStation`](xref:Z21.Core.IZ21CommandStation) —
for every command, handler, capability, and model.
