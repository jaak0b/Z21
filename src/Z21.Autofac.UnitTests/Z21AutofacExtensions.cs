using Autofac;
using CommandStation;
using CommandStation.Transport;
using Z21.Core;
using Z21.Core.ResponseHandler.Settings;
using Z21.Core.ResponseHandler.SystemState;
using Z21.Core.ResponseParser;

namespace Z21.Autofac.UnitTests
{
  [TestFixture]
  public class Z21AutofacExtensionsTests
  {
    private IContainer BuildContainer(Action<ContainerBuilder> configure)
    {
      var builder = new ContainerBuilder();
      configure(builder);
      return builder.Build();
    }

    [Test]
    public void AddZ21ResponseHandler_RegistersTypesCorrectly()
    {
      using var container = BuildContainer(containerBuilder => containerBuilder.AddZ21());

      var handler = container.Resolve<IHardwareInfoResponseHandler>();
      Assert.That(handler, Is.InstanceOf<HardwareInfoResponseHandler>());

      var handlerType = container.Resolve<HardwareInfoResponseHandler>();
      Assert.That(handlerType, Is.InstanceOf<HardwareInfoResponseHandler>());
      Assert.That(handlerType, Is.EqualTo(handler));
    }

    [Test]
    public void AddZ21ResponseHandler_DiscoversAccessoryModeHandler()
    {
      using var container = BuildContainer(containerBuilder => containerBuilder.AddZ21());

      var handler = container.Resolve<IAccessoryModeResponseHandler>();
      Assert.That(handler, Is.InstanceOf<AccessoryModeResponseHandler>());
    }

    [Test]
    public void AddZ21ResponseParser_Registers_All_Parser_Types()
    {
      using var container = BuildContainer(containerBuilder => containerBuilder.AddZ21());

      var baseInterface = typeof(IZ21ResponseParser);
      var parserTypes = baseInterface.Assembly
                                     .GetTypes()
                                     .Where(type => type.IsClass && !type.IsAbstract && baseInterface.IsAssignableFrom(type))
                                     .ToList();

      foreach (var parserType in parserTypes)
      {
        var instance = container.Resolve(parserType);
        Assert.NotNull(instance, "Parser type should be resolvable: " + parserType.Name);

        var interfaces = parserType.GetInterfaces()
                                   .Where(i => baseInterface.IsAssignableFrom(i) && i != baseInterface);

        foreach (var serviceType in interfaces)
        {
          var ifaceInstance = container.Resolve(serviceType);
          Assert.NotNull(ifaceInstance, "Interface should be resolvable: " + serviceType.Name);
          Assert.That(ifaceInstance, Is.SameAs(instance), "Interface should resolve to same singleton instance");
        }
      }
    }

    [Test]
    public void AddZ21_Registers_Transport_As_Singleton()
    {
      using var container = BuildContainer(containerBuilder => containerBuilder.AddZ21());

      var t1 = container.Resolve<ITransport>();
      var t2 = container.Resolve<ITransport>();

      Assert.That(t1, Is.Not.Null);
      Assert.That(t2, Is.SameAs(t1), "Transport should be singleton");
    }

    [Test]
    public void AddZ21_Registers_CommandStation_As_Singleton()
    {
      using var container = BuildContainer(containerBuilder => containerBuilder.AddZ21());

      var s1 = container.Resolve<ICommandStation>();
      var s2 = container.Resolve<IZ21CommandStation>();

      Assert.That(s1, Is.Not.Null);
      Assert.That(s2, Is.SameAs(s1), "ICommandStation and IZ21CommandStation should resolve to the same singleton");
    }

    [Test]
    public void AddZ21_Registers_Options_Instance()
    {
      using var container = BuildContainer(containerBuilder => containerBuilder.AddZ21(optionsConfiguration: options => options.KeepAliveInterval = TimeSpan.FromSeconds(5)));

      var options = container.Resolve<Z21Options>();

      Assert.That(options, Is.Not.Null);
      Assert.That(options.KeepAliveInterval, Is.EqualTo(TimeSpan.FromSeconds(5)));
    }
  }
}
