using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.SystemState;

namespace Z21.UnitTest.Core.ResponseHandler.SystemState
{
  //Test response from z21: 0C-00-1A-00-04-02-00-00-43-01-00-00

  public class HardwareInfoResponseHandlerTest
  {
    private HardwareInfoResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x0C, 0x00, 0x1A, 0x00, 0x04, 0x02, 0x00, 0x00, 0x43, 0x01, 0x00, 0x00];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x0C, 0x00, 0x1A, 0x01, 0x04, 0x02, 0x00, 0x00, 0x43, 0x01, 0x00, 0x00 }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs()
    {
      byte[] response = [0x0C, 0x00, 0x1A, 0x00, 0x04, 0x02, 0x00, 0x00, 0x43, 0x01, 0x00, 0x00];

      HardwareInfoEventArgs? receivedArgs = null;
      HardwareInfoResponseHandler? handler = null;
      _handler.OnHardwareInfoReceived += (sender, args) =>
                                         {
                                           receivedArgs = args;
                                           handler = sender as HardwareInfoResponseHandler;
                                         };

      _handler.Handle(response);

      Assert.That(handler, Is.EqualTo(_handler));
      Assert.That(receivedArgs, Is.Not.Null);
      Assert.That(receivedArgs.Z21HardwareType, Is.EqualTo(Z21HardwareType.z21Start));
    }
  }
}