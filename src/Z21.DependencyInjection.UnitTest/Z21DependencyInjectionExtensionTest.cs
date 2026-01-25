using Microsoft.Extensions.DependencyInjection;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.SystemState;

namespace Z21.DependencyInjection.UnitTest
{
  public class Z21DependencyInjectionExtensionTest
  {
    [Test]
    public void AddZ21ResponseHandler_SameInstanceIsRegisteredForAllInterfaces()
    {
      ServiceCollection services = new();
      services.AddZ21ResponseParser();
      services.AddZ21ResponseHandler();
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
  }
}