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
    public static IServiceCollection AddZ21(this IServiceCollection services, Action<Z21Configuration>? configurationAction = null)
    {
      services.AddSingleton<IZ21Transport, Z21Transport>();
      services.AddSingleton<IZ21Client, Z21Client>();
      services.AddActivatedSingleton<Z21ResponseHandler>();
      
      services.ConfigureZ21Client(configurationAction);
      services.AddZ21ResponseParser();
      services.AddZ21ResponseHandler();
      return services;
    }

    /// <summary>
    /// Discovers all Z21 response handlers and registers them in the <paramref name="services"/> collection.
    /// </summary>
    private static IServiceCollection AddZ21ResponseHandler(this IServiceCollection services)
    {
      ArgumentNullException.ThrowIfNull(services);

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

    private static IServiceCollection AddZ21ResponseParser(this IServiceCollection services)
    {
      ArgumentNullException.ThrowIfNull(services);

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

    private static IServiceCollection ConfigureZ21Client(this IServiceCollection services, Action<Z21Configuration>? configurationAction = null) // TODO: Test
    {
      ArgumentNullException.ThrowIfNull(services);

      Z21Configuration configuration = new();
      configurationAction?.Invoke(configuration);
      services.AddSingleton(configuration);

      return services;
    }
  }
}