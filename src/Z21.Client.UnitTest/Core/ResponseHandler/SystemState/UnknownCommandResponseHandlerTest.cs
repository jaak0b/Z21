using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.SystemState;

namespace Z21.UnitTest.Core.ResponseHandler.SystemState
{
  public class UnknownCommandResponseHandlerTest
  {
    private UnknownCommandResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x00, 0x00, 0x40, 0x00, 0x61, 0x82, 0xE3];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x00, 0x00, 0x40, 0x00, 0x62, 0x82, 0xE3 }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs()
    {
      byte[] response = [0x00, 0x00, 0x40, 0x00, 0x62, 0x82, 0xE3];

      UnknownCommandReceivedEventArgs? receivedArgs = null;
      UnknownCommandResponseHandler? handler = null;
      _handler.OnUnknownCommandReceived += (sender, args) =>
                                           {
                                             receivedArgs = args;
                                             handler = sender as UnknownCommandResponseHandler;
                                           };

      _handler.Handle(response);

      Assert.That(receivedArgs, Is.Not.Null);
      Assert.That(receivedArgs.UnknownDatagram, Is.EqualTo(response));
      Assert.That(handler, Is.EqualTo(_handler));
    }
  }
}