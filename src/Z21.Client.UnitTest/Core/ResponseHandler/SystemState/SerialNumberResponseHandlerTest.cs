using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.SystemState;

namespace Z21.UnitTest.Core.ResponseHandler.SystemState
{
  public class SerialNumberResponseHandlerTest
  {
    private SerialNumberResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] response = new byte[8];
      response[2] = 0x10;
      response[3] = 0x00;

      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x00, 0x00, 0x11, 0x00 }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEventWithCorrectSerialNumber()
    {
      byte[] response = new byte[8];
      const uint expectedSerial = 123456789;
      BitConverter.GetBytes(expectedSerial).CopyTo(response, 4);

      SerialNumberReceivedEventArgs? receivedArgs = null;

      _handler.OnSerialNumberReceived += (sender, args) => { receivedArgs = args; };

      _handler.Handle(response);
      Assert.That(receivedArgs, Is.Not.Null);
      Assert.That(receivedArgs.SerialNumber, Is.EqualTo(expectedSerial));
    }
  }
}