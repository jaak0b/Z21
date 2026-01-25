using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.Settings;
using Z21.Core.ResponseHandler.SystemState;

namespace Z21.UnitTest.Core.ResponseHandler.Settings
{
  public class LocoModeResponseHandlerTest
  {
    private LocoModeResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x07, 0x00, 0x60, 0x00, 0x00, 0x0C, 0x00];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x07, 0x00, 0x61, 0x00, 0x00, 0x0C, 0x00 }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    [TestCase(0x00, DecoderMode.DCC)]
    [TestCase(0x01, DecoderMode.MM)]
    [TestCase(0x05, DecoderMode.Unknown)]
    [TestCase(0x24, DecoderMode.Unknown)]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs(byte modeByte, DecoderMode modeEnum)
    {
      byte[] response = [0x07, 0x00, 0x60, 0x00, 0x00, 0x0C, modeByte];

      DecoderModeReceivedEventArgs? receivedArgs = null;
      LocoModeResponseHandler? handler = null;
      _handler.OnLocoModeReceived += (sender, args) =>
                                     {
                                       receivedArgs = args;
                                       handler = sender as LocoModeResponseHandler;
                                     };

      _handler.Handle(response);

      Assert.That(handler, Is.EqualTo(_handler));
      Assert.That(receivedArgs, Is.Not.Null);
      Assert.That(receivedArgs.LocoAddress, Is.EqualTo(12));
      Assert.That(receivedArgs.Mode, Is.EqualTo(modeEnum));
    }
  }
}