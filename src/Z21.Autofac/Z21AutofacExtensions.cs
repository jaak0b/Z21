using Autofac;
using CommandStation;
using CommandStation.Framing;
using CommandStation.Transport;
using CommandStation.Transport.Udp;
using Z21.Core;
using Z21.Core.Codecs;
using Z21.Core.Command;
using Z21.Core.Framing;
using Z21.Core.Reflection;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseParser;

namespace Z21.Autofac
{
  public static class Z21AutofacExtensions
  {

    public static ContainerBuilder AddZ21(this ContainerBuilder builder, Action<UdpTransportOptions>? transportConfiguration = null, Action<Z21Options>? optionsConfiguration = null)
    {
      UdpTransportOptions transportOptions = new();
      transportConfiguration?.Invoke(transportOptions);
      builder.RegisterInstance(transportOptions).AsSelf().SingleInstance();

      builder.RegisterType<UdpTransport>().As<ITransport>().SingleInstance();
      builder.RegisterType<Z21FrameReader>().As<IFrameReader>().SingleInstance();
      builder.RegisterType<Z21FrameBuilder>().As<IZ21FrameBuilder>().SingleInstance();
      builder.RegisterType<AddressCodec>().As<IAddressCodec>().SingleInstance();
      builder.RegisterType<LocoSpeedCodec>().As<ILocoSpeedCodec>().SingleInstance();
      builder.RegisterType<Z21CommandFactory>().As<IZ21CommandFactory>().SingleInstance();
      builder.RegisterType<Z21CommandStation>().As<IZ21CommandStation>().As<ICommandStation>().SingleInstance();
      builder.RegisterType<Z21ResponseHandler>().AsSelf().SingleInstance();

      builder.ConfigureZ21Options(optionsConfiguration);
      builder.AddZ21ResponseParser();
      builder.AddZ21ResponseHandler();
      return builder;
    }

    /// <summary>
    /// Discovers all Z21 response handlers and registers them in the <paramref name="builder"/> container.
    /// </summary>
    private static ContainerBuilder AddZ21ResponseHandler(this ContainerBuilder builder) =>
      builder.AddDiscovered(typeof(IZ21ResponseHandler), includeBaseInterface: true);

    private static ContainerBuilder AddZ21ResponseParser(this ContainerBuilder builder) =>
      builder.AddDiscovered(typeof(IZ21ResponseParser), includeBaseInterface: false);

    private static ContainerBuilder AddDiscovered(this ContainerBuilder builder, Type baseInterface, bool includeBaseInterface)
    {
      ArgumentNullException.ThrowIfNull(builder);

      Z21ServiceDiscovery discovery = new();

      foreach (Type implementationType in discovery.GetImplementations(baseInterface))
      {
        builder.RegisterType(implementationType)
               .AsSelf()
               .SingleInstance();

        foreach (Type serviceType in discovery.GetServiceInterfaces(implementationType, baseInterface, includeBaseInterface))
        {
          builder.Register(ctx => ctx.Resolve(implementationType))
                 .As(serviceType)
                 .SingleInstance();
        }
      }

      return builder;
    }

    private static ContainerBuilder ConfigureZ21Options(this ContainerBuilder builder, Action<Z21Options>? optionsConfiguration = null)
    {
      ArgumentNullException.ThrowIfNull(builder);

      Z21Options options = new();
      optionsConfiguration?.Invoke(options);

      builder.RegisterInstance(options)
             .As<Z21Options>()
             .SingleInstance();

      return builder;
    }
  }
}
