using CommandStation;
using CommandStation.Framing;
using CommandStation.Transport;
using CommandStation.Transport.Udp;
using Microsoft.Extensions.DependencyInjection;
using Z21.Core;
using Z21.Core.Codecs;
using Z21.Core.Command;
using Z21.Core.Framing;
using Z21.Core.Reflection;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseParser;

namespace Z21.DependencyInjection
{

  public static class Z21DependencyInjectionExtension
  {
    public static IServiceCollection AddZ21(this IServiceCollection services, Action<UdpTransportOptions>? transportConfiguration = null, Action<Z21Options>? optionsConfiguration = null)
    {
      services.AddSingleton(_ =>
                            {
                              UdpTransportOptions options = new();
                              transportConfiguration?.Invoke(options);
                              return options;
                            });
      services.AddSingleton<ITransport, UdpTransport>();
      services.AddSingleton<IFrameReader, Z21FrameReader>();
      services.AddSingleton<IZ21FrameBuilder, Z21FrameBuilder>();
      services.AddSingleton<IAddressCodec, AddressCodec>();
      services.AddSingleton<ILocoSpeedCodec, LocoSpeedCodec>();
      services.AddSingleton<IZ21CommandFactory, Z21CommandFactory>();
      services.AddSingleton<IZ21CommandStation, Z21CommandStation>();
      services.AddSingleton<ICommandStation>(provider => provider.GetRequiredService<IZ21CommandStation>());
      services.AddSingleton<Z21ResponseHandler>();

      services.ConfigureZ21Options(optionsConfiguration);
      services.AddZ21ResponseParser();
      services.AddZ21ResponseHandler();
      return services;
    }

    /// <summary>
    /// Discovers all Z21 response handlers and registers them in the <paramref name="services"/> collection.
    /// </summary>
    private static IServiceCollection AddZ21ResponseHandler(this IServiceCollection services) =>
      services.AddDiscovered(typeof(IZ21ResponseHandler), includeBaseInterface: true);

    private static IServiceCollection AddZ21ResponseParser(this IServiceCollection services) =>
      services.AddDiscovered(typeof(IZ21ResponseParser), includeBaseInterface: false);

    private static IServiceCollection AddDiscovered(this IServiceCollection services, Type baseInterface, bool includeBaseInterface)
    {
      ArgumentNullException.ThrowIfNull(services);

      Z21ServiceDiscovery discovery = new();

      foreach (Type implementationType in discovery.GetImplementations(baseInterface))
      {
        services.AddSingleton(implementationType);
        foreach (Type serviceType in discovery.GetServiceInterfaces(implementationType, baseInterface, includeBaseInterface))
          services.AddSingleton(serviceType, provider => provider.GetRequiredService(implementationType));
      }

      return services;
    }

    private static IServiceCollection ConfigureZ21Options(this IServiceCollection services, Action<Z21Options>? optionsConfiguration)
    {
      ArgumentNullException.ThrowIfNull(services);

      Z21Options options = new();
      optionsConfiguration?.Invoke(options);
      services.AddSingleton(options);

      return services;
    }
  }
}
