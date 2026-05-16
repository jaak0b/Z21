using Autofac;
using Z21.Core;
using Z21.Core.Model;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseParser;
using Z21.Transport;

namespace Z21.Autofac
{
  public static class Z21AutofacExtensions
  {

    public static ContainerBuilder AddZ21(this ContainerBuilder builder, Action<Z21Configuration>? configurationAction = null)
    {
      builder.RegisterType<Z21Transport>().As<IZ21Transport>().SingleInstance();
      builder.RegisterType<Z21Client>().As<IZ21Client>().SingleInstance();
      builder.RegisterType<Z21ResponseHandler>().AsSelf().SingleInstance().AutoActivate();

      builder.ConfigureZ21Client(configurationAction);
      builder.AddZ21ResponseParser();
      builder.AddZ21ResponseHandler();
      return builder;
    }

    /// <summary>
    /// Discovers all Z21 response handlers and registers them in the <paramref name="builder"/> container.
    /// </summary>
    private static ContainerBuilder AddZ21ResponseHandler(this ContainerBuilder builder)
    {
      ArgumentNullException.ThrowIfNull(builder);

      Type baseInterface = typeof(IZ21ResponseHandler);

      IEnumerable<Type> handlerTypes =
        baseInterface.Assembly
                     .GetTypes()
                     .Where(t => t is { IsClass: true, IsAbstract: false } && baseInterface.IsAssignableFrom(t));

      foreach (Type handlerType in handlerTypes)
      {
        builder.RegisterType(handlerType)
               .AsSelf()
               .SingleInstance();

        List<Type> interfacesToRegister =
          handlerType.GetInterfaces()
                     .Where(baseInterface.IsAssignableFrom)
                     .ToList();

        foreach (Type serviceType in interfacesToRegister)
        {
          builder.Register(ctx => ctx.Resolve(handlerType))
                 .As(serviceType)
                 .SingleInstance();
        }
      }

      return builder;
    }

    private static ContainerBuilder AddZ21ResponseParser(this ContainerBuilder builder)
    {
      ArgumentNullException.ThrowIfNull(builder);

      Type baseInterface = typeof(IZ21ResponseParser);

      IEnumerable<Type> parserTypes =
        baseInterface.Assembly
                     .GetTypes()
                     .Where(t => t is { IsClass: true, IsAbstract: false } && baseInterface.IsAssignableFrom(t));

      foreach (Type parserType in parserTypes)
      {
        builder.RegisterType(parserType)
               .AsSelf()
               .SingleInstance();

        List<Type> interfacesToRegister =
          parserType.GetInterfaces()
                    .Where(i => baseInterface.IsAssignableFrom(i) && i != baseInterface)
                    .ToList();

        foreach (Type serviceType in interfacesToRegister)
        {
          builder.Register(ctx => ctx.Resolve(parserType))
                 .As(serviceType)
                 .SingleInstance();
        }
      }

      return builder;
    }

    private static ContainerBuilder ConfigureZ21Client(this ContainerBuilder builder, Action<Z21Configuration>? configurationAction = null)
    {
      ArgumentNullException.ThrowIfNull(builder);

      var config = new Z21Configuration();
      configurationAction?.Invoke(config);

      builder.RegisterInstance(config)
             .As<Z21Configuration>()
             .SingleInstance();

      return builder;
    }
  }
}