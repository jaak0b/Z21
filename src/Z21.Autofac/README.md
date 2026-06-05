# Z21.Autofac

Autofac wiring for the [Z21](https://www.nuget.org/packages/Z21) client. If you use
Autofac instead of Microsoft.Extensions.DependencyInjection, this package gives you the
same one-line setup: register the command station, the UDP transport and all protocol
handlers, then resolve `IZ21CommandStation`.

```csharp
var builder = new ContainerBuilder();
builder.AddZ21(t => t.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.111"), 21105));

var container = builder.Build();
var station = container.Resolve<IZ21CommandStation>();

await station.ConnectAsync();
await station.TrackPowerOnAsync();
```

See the [project page](https://github.com/jaak0b/Z21) for the full API. Licensed under GPL-3.0.
