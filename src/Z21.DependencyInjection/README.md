# Z21.DependencyInjection

Microsoft.Extensions.DependencyInjection wiring for the [Z21](https://www.nuget.org/packages/Z21)
client. One call registers the command station, the UDP transport and every protocol
handler, so you can resolve `IZ21CommandStation` and start talking to your Z21.

```csharp
var services = new ServiceCollection();
services.AddZ21(t => t.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.111"), 21105));

await using var provider = services.BuildServiceProvider();
var station = provider.GetRequiredService<IZ21CommandStation>();

await station.ConnectAsync();
await station.DriveAsync(3, DccSpeedMode.Steps128, DrivingDirection.Forward, 40);
```

`AddZ21` also takes an optional second configurator for protocol options such as the
broadcast flags to subscribe to. See the [project page](https://github.com/jaak0b/Z21)
for details. Licensed under GPL-3.0.
