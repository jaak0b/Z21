using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.SystemState;

namespace Z21.UnitTest.Core.ResponseHandler.SystemState
{
  public class FirmwareVersionResponseHandlerTest
  {
    private FirmwareVersionResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x00, 0x00, 0x40, 0x00, 0xf3, 0x0a, 0x01, 0x23, 0xdb];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x00, 0x00, 0x40, 0x00, 0x23, 0x0a, 0x01, 0x23, 0xdb }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs()
    {
      byte[] response = [0x00, 0x00, 0x40, 0x00, 0xf3, 0x0a, 0x01, 0x23, 0xdb];

      FirmwareVersionReceivedEventArgs? receivedArgs = null;
      FirmwareVersionResponseHandler? handler = null;
      _handler.OnFirmwareVersionReceived += (sender, args) =>
                                            {
                                              receivedArgs = args;
                                              handler = sender as FirmwareVersionResponseHandler;
                                            };

      _handler.Handle(response);

      Assert.That(handler, Is.EqualTo(_handler));
      Assert.That(receivedArgs, Is.Not.Null);
      Assert.That(receivedArgs.FirmwareVersion, Is.Not.Null);
      Assert.That(receivedArgs.FirmwareVersion.Major, Is.EqualTo(1));
      Assert.That(receivedArgs.FirmwareVersion.Minor, Is.EqualTo(23));
      Assert.That(receivedArgs.FirmwareVersion.Firmware, Is.EqualTo("1.23"));
    }
  }
}