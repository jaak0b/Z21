# CommandStation.Abstractions

The protocol- and transport-neutral contracts shared by the model-railway command-station
libraries. It has no dependency on any particular command station or wire format, so you
can code against the interfaces and swap implementations underneath.

It defines the domain API (`ICommandStation` plus capability interfaces like `ILocoControl`,
`IAccessoryControl`, `ITrackPowerControl` and `ISystemInfoProvider`), the transport and
framing abstractions (`ITransport`, `IFrameReader`), and the shared domain model
(`LocoInfoData`, `SystemState`, `DccSpeedMode`, and friends).

You normally don't install this directly — it comes in as a dependency of an
implementation such as [Z21](https://www.nuget.org/packages/Z21). Reach for it when you
want to write code (or your own command station) against the neutral abstractions.

See the [project page](https://github.com/jaak0b/Z21). Licensed under GPL-3.0.
