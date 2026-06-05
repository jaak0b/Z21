using CommandStation;
using CommandStation.Model;
using CommandStation.Transport;
using Microsoft.Extensions.DependencyInjection;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.Settings;
using Z21.Core.ResponseHandler.SystemState;

namespace Z21.DependencyInjection.UnitTest
{
  public class Z21DependencyInjectionExtensionTest
  {
    [Test]
    public void AddZ21_WithoutHost_ResolvingCommandStation_WiresInboundHandling()
    {
      ServiceCollection services = new();
      services.AddZ21();
      SpyTransport transport = new();
      services.AddSingleton<ITransport>(transport);
      ServiceProvider serviceProvider = services.BuildServiceProvider();

      ILocoControl station = serviceProvider.GetRequiredService<ICommandStation>() as ILocoControl
                             ?? throw new InvalidOperationException("Station does not support loco control.");
      LocoInfoData? received = null;
      station.LocoInfoReceived += (_, data) => received = data;

      transport.RaiseBytes([0x0F, 0x00, 0x40, 0x00, 0xEF, 0x00, 0x03, 0x02, 0x87, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69]);

      Assert.That(received, Is.Not.Null);
      Assert.That(received!.LocoAddress, Is.EqualTo(3));
    }

    [Test]
    public void AddZ21_WithoutHost_NewHandler_IsDiscoveredAndFlowsThroughCapability()
    {
      ServiceCollection services = new();
      services.AddZ21();
      SpyTransport transport = new();
      services.AddSingleton<ITransport>(transport);
      ServiceProvider serviceProvider = services.BuildServiceProvider();

      IFastClockControl station = serviceProvider.GetRequiredService<ICommandStation>() as IFastClockControl
                                  ?? throw new InvalidOperationException("Station does not support fast clock control.");
      ModelTime? received = null;
      station.ModelTimeChanged += (_, time) => received = time;

      // LAN_FAST_CLOCK_DATA: day=0, hour=12, minute=30, second=45, rate=8
      transport.RaiseBytes([0x0C, 0x00, 0xCD, 0x00, 0x66, 0x25, 0x0C, 0x1E, 0x2D, 0x08, 0x80, 0x00]);

      Assert.That(received, Is.Not.Null);
      Assert.Multiple(() =>
                      {
                        Assert.That(received!.Hour, Is.EqualTo(12));
                        Assert.That(received.Minute, Is.EqualTo(30));
                        Assert.That(received.Rate, Is.EqualTo(8));
                      });
    }

    [Test]
    public void AddZ21_ProviderDisposedSynchronously_DoesNotThrow()
    {
      ServiceCollection services = new();
      services.AddZ21();
      ServiceProvider serviceProvider = services.BuildServiceProvider();

      // Resolving the station instantiates the singleton UdpTransport, so the provider tracks it for disposal.
      _ = serviceProvider.GetRequiredService<ICommandStation>();

      Assert.DoesNotThrow(() => serviceProvider.Dispose());
    }

    [Test]
    public void AddZ21_WithZ21ResponseHandlers_SameInstanceIsRegisteredForAllInterfaces()
    {
      ServiceCollection services = new();
      services.AddZ21();
      ServiceProvider serviceProvider = services.BuildServiceProvider();

      ISerialNumberResponseHandler implementation =
        serviceProvider.GetRequiredService<SerialNumberResponseHandler>();
      ISerialNumberResponseHandler implementationSpecificInterface =
        serviceProvider.GetRequiredService<ISerialNumberResponseHandler>();
      ISerialNumberResponseHandler baseInterface = serviceProvider
                                                        .GetRequiredService<IEnumerable<IZ21ResponseHandler>>()
                                                        .OfType<ISerialNumberResponseHandler>()
                                                        .Single();

      Assert.Multiple(() =>
                      {
                        Assert.That(implementation, Is.SameAs(implementationSpecificInterface));
                        Assert.That(implementation, Is.SameAs(baseInterface));
                        Assert.That(implementationSpecificInterface, Is.SameAs(baseInterface));
                      });
    }

    [Test]
    public void AddZ21_WithoutHost_AccessoryModeFrame_IsDiscoveredAndDispatched()
    {
      ServiceCollection services = new();
      services.AddZ21();
      SpyTransport transport = new();
      services.AddSingleton<ITransport>(transport);
      ServiceProvider serviceProvider = services.BuildServiceProvider();

      // Resolving the station wires the dispatcher to the transport.
      _ = serviceProvider.GetRequiredService<ICommandStation>();
      IAccessoryModeResponseHandler handler = serviceProvider.GetRequiredService<IAccessoryModeResponseHandler>();
      DecoderModeReceivedEventArgs? received = null;
      handler.OnAccessoryModeReceived += (_, args) => received = args;

      // LAN_GET_TURNOUTMODE: address=12 (0x000C), mode=DCC (0x00)
      transport.RaiseBytes([0x07, 0x00, 0x70, 0x00, 0x00, 0x0C, 0x00]);

      Assert.That(received, Is.Not.Null);
      Assert.Multiple(() =>
                      {
                        Assert.That(received!.LocoAddress, Is.EqualTo(12));
                        Assert.That(received.Mode, Is.EqualTo(DecoderMode.DCC));
                      });
    }
  }
}