using System;
using System.Linq;
using Z21.Core.Reflection;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.Driving;
using Z21.Core.ResponseParser;

namespace Z21.UnitTest.Core.Reflection
{
  public class Z21ServiceDiscoveryTest
  {
    private Z21ServiceDiscovery _discovery = null!;

    [SetUp]
    public void SetUp() => _discovery = new Z21ServiceDiscovery();

    [Test]
    public void GetImplementations_ReturnsConcreteHandlersOnly()
    {
      Type[] implementations = _discovery.GetImplementations(typeof(IZ21ResponseHandler)).ToArray();

      Assert.Multiple(() =>
                      {
                        Assert.That(implementations, Has.Member(typeof(LocoInfoResponseHandler)), "concrete handler must be discovered");
                        Assert.That(implementations, Has.None.Matches<Type>(t => t.IsAbstract), "abstract types must be excluded");
                        Assert.That(implementations, Has.None.Matches<Type>(t => t.IsInterface), "interfaces must be excluded");
                        Assert.That(implementations, Has.None.Matches<Type>(t => !typeof(IZ21ResponseHandler).IsAssignableFrom(t)), "all must implement the base interface");
                      });
    }

    [Test]
    public void GetServiceInterfaces_IncludeBaseTrue_ContainsBaseInterface()
    {
      Type[] interfaces = _discovery.GetServiceInterfaces(typeof(LocoInfoResponseHandler), typeof(IZ21ResponseHandler), includeBaseInterface: true).ToArray();

      Assert.Multiple(() =>
                      {
                        Assert.That(interfaces, Has.Member(typeof(IZ21ResponseHandler)), "base interface included when flag is true");
                        Assert.That(interfaces, Has.Member(typeof(ILocoInfoResponseHandler)), "specific interface always included");
                      });
    }

    [Test]
    public void GetServiceInterfaces_IncludeBaseFalse_ExcludesBaseInterface()
    {
      Type[] interfaces = _discovery.GetServiceInterfaces(typeof(SystemStateResponseParser), typeof(IZ21ResponseParser), includeBaseInterface: false).ToArray();

      Assert.Multiple(() =>
                      {
                        Assert.That(interfaces, Has.None.EqualTo(typeof(IZ21ResponseParser)), "base interface excluded when flag is false");
                        Assert.That(interfaces, Has.Member(typeof(ISystemStateResponseParser)), "specific interface still included");
                      });
    }
  }
}
