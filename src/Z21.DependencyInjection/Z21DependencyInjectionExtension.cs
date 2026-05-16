using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Z21.Core;
using Z21.Core.Model;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseParser;
using Z21.Transport;

namespace Z21.DependencyInjection
{
  public static class Z21DependencyInjectionExtension
  {
    public static ServiceCollection AddZ21(this ServiceCollection services, IPEndPoint z21EndPoint, Action<Z21Configuration>? configurationAction = null)
    {
      services.ConfigureZ21Client(z21EndPoint, configurationAction);
      services.AddZ21ResponseParser();
      services.AddZ21ResponseHandler();
      services.AddZ21Transport();
      services.AddZ21Client();
      return services;
    }

    /// <summary>
    /// Discovers all Z21 response handlers and registers them in the <paramref name="services"/> collection.
    /// </summary>
    public static ServiceCollection AddZ21ResponseHandler(this ServiceCollection services)
    {
      ArgumentNullException.ThrowIfNull(services, nameof(services));

      Type baseInterface = typeof(IZ21ResponseHandler);

      IEnumerable<Type> handlerTypes = baseInterface.Assembly.GetTypes().Where(type => type is { IsClass: true, IsAbstract: false } && baseInterface.IsAssignableFrom(type));

      foreach (Type handlerType in handlerTypes)
      {
        // Get all interfaces this class implements that are in the chain to IZ21ResponseHandler
        List<Type> interfacesToRegister = handlerType.GetInterfaces().Where(baseInterface.IsAssignableFrom).ToList();
        services.AddSingleton(handlerType);
        foreach (Type serviceType in interfacesToRegister)
        {
          services.AddSingleton(serviceType, provider => provider.GetRequiredService(handlerType));
        }
      }

      return services;
    }

    public static ServiceCollection AddZ21ResponseParser(this ServiceCollection services)
    {
      ArgumentNullException.ThrowIfNull(services, nameof(services));

      Type baseInterface = typeof(IZ21ResponseParser);

      IEnumerable<Type> handlerTypes = baseInterface.Assembly.GetTypes().Where(type => type is { IsClass: true, IsAbstract: false } && baseInterface.IsAssignableFrom(type));

      foreach (Type handlerType in handlerTypes)
      {
        // Get all interfaces this class implements that are in the chain up to IZ21ResponseParser
        List<Type> interfacesToRegister = handlerType.GetInterfaces().Where(type => baseInterface.IsAssignableFrom(type) && type != baseInterface).ToList();
        services.AddSingleton(handlerType);
        foreach (Type serviceType in interfacesToRegister)
        {
          services.AddSingleton(serviceType, provider => provider.GetRequiredService(handlerType));
        }
      }

      return services;
    }

    public static ServiceCollection AddZ21Transport(this ServiceCollection services) // TODO: Test
    {
      ArgumentNullException.ThrowIfNull(services, nameof(services));
      services.AddSingleton<IZ21Transport, Z21Transport>();
      return services;
    }

    public static ServiceCollection AddZ21Client(this ServiceCollection services) // TODO: Test
    {
      ArgumentNullException.ThrowIfNull(services, nameof(services));
      services.AddSingleton<IZ21Client, Z21Client>();
      return services;
    }

    public static ServiceCollection ConfigureZ21Client(this ServiceCollection services, IPEndPoint z21EndPoint, Action<Z21Configuration>? configurationAction = null) // TODO: Test
    {
      ArgumentNullException.ThrowIfNull(services, nameof(services));
      ArgumentNullException.ThrowIfNull(z21EndPoint, nameof(z21EndPoint));

      Z21Configuration configuration = new(z21EndPoint);
      configurationAction?.Invoke(configuration);
      services.AddSingleton(configuration);

      return services;
    }
  }
}