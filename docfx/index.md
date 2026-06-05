# Z21 Client Library

A platform-independent, event-driven **C# client for the ROCO/Fleischmann Z21 LAN
protocol (V1.13)** over UDP. This site hosts the **API reference**, generated directly
from the library's source XML documentation comments, plus a short
[Getting Started](articles/getting-started.md) guide.

## Highlights

- **Protocol-agnostic public API** — the headline interface is `ICommandStation` with
  small, focused capability interfaces (`ILocoControl`, `IAccessoryControl`,
  `ITrackPowerControl`, `ISystemInfoProvider`, `IProgrammingControl`, `IFeedbackControl`,
  `IFastClockControl`). Transport (UDP) and protocol (Z21) are cleanly decoupled.
- **Event-driven** — subscribe to typed events such as `LocoInfoReceived`,
  `SystemStateReceived`, `TrackPowerChanged`.
- **Dependency injection out of the box** — `AddZ21(...)` for both
  `Microsoft.Extensions.DependencyInjection` and Autofac.
- **Complete protocol coverage** — System, Driving, Switching, CV/POM programming, R-BUS
  feedback, RailCom, LocoNet gateway, CAN, fast clock, and the zLink
  booster/decoder/adapter messages.

## Install

```bash
dotnet add package Z21
dotnet add package Z21.DependencyInjection   # or Z21.Autofac
```

## Quick start

```csharp
using System.Net;
using CommandStation;
using Microsoft.Extensions.DependencyInjection;
using Z21.Core;
using Z21.DependencyInjection;

var services = new ServiceCollection();
services.AddZ21(transport =>
    transport.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.111"), 21105));

await using var provider = services.BuildServiceProvider();
var station = provider.GetRequiredService<IZ21CommandStation>();

station.LocoInfoReceived += (_, loco) =>
    Console.WriteLine($"Loco {loco.LocoAddress} @ {loco.LocoSpeed}");

await station.ConnectAsync();
await station.TrackPowerOnAsync();
await station.DriveAsync(13, DccSpeedMode.Steps128, DrivingDirection.Forward, 40);
```

See [Getting Started](articles/getting-started.md) for the full walkthrough, or browse
the API reference starting at [`ICommandStation`](xref:CommandStation.ICommandStation).

## Protocol specification

The official Z21 LAN protocol documentation is available from ROCO in
[English](https://www.z21.eu/media/Kwc_Basic_DownloadTag_Component/root-en-main_47-1652-959-downloadTag-download/default/d559b9cf/1628743384/z21-lan-protokoll-en.pdf)
and
[German](https://www.z21.eu/media/Kwc_Basic_DownloadTag_Component/47-1652-959-downloadTag/default/69bad87e/1699290251/z21-lan-protokoll.pdf).

This project is licensed under **GPL-3.0**.
