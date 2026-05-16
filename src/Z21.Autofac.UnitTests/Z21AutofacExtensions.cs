using System.Net;
using Autofac;
using Z21.Core;
using Z21.Core.Model;
using Z21.Core.ResponseHandler.SystemState;
using Z21.Core.ResponseParser;
using Z21.Transport;

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
    public void AddZ21Transport_Registers_Transport_As_Singleton()
    {
      using var container = BuildContainer(containerBuilder => containerBuilder.AddZ21());
      
      var t1 = container.Resolve<IZ21Transport>();
      var t2 = container.Resolve<IZ21Transport>();

      Assert.That(t1, Is.Not.Null);
      Assert.That(t2, Is.Not.Null);
      Assert.That(t2, Is.SameAs(t1), "Transport should be singleton");
    }

    [Test]
    public void AddZ21Client_Registers_Client_As_Singleton()
    {
      using var container = BuildContainer(containerBuilder => containerBuilder.AddZ21());

      var c1 = container.Resolve<IZ21Client>();
      var c2 = container.Resolve<IZ21Client>();

      Assert.NotNull(c1);
      Assert.NotNull(c2);
      Assert.That(c2, Is.SameAs(c1), "Client should be singleton");
    }

    [Test]
    public void ConfigureZ21Client_Registers_Configuration_Instance()
    {
      using var container = BuildContainer(containerBuilder => containerBuilder.AddZ21(cfg => cfg.ResponseTime = TimeSpan.FromSeconds(5)));

      var config = container.Resolve<Z21Configuration>();

      Assert.NotNull(config);
      Assert.That(config.ResponseTime, Is.EqualTo(TimeSpan.FromSeconds(5)));
    }
  }
}