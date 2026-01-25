using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.SystemState;

namespace Z21.UnitTest.Core.ResponseHandler.SystemState
{
  public class BroadcastFlagsResponseHandlerTest
  {
    private BroadcastFlagsResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x00, 0x00, 0x51, 0x00];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x00, 0x00, 0x52, 0x00 }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs()
    {
      byte[] response = [0x08, 0x00, 0x51, 0x00, 0x01, 0x00, 0x01, 0x00];

      BroadcastFlagsReceivedEventArgs? receivedArgs = null;
      BroadcastFlagsResponseHandler? handler = null;
      _handler.OnBroadcastFlagsReceived += (sender, args) =>
                                           {
                                             receivedArgs = args;
                                             handler = sender as BroadcastFlagsResponseHandler;
                                           };

      _handler.Handle(response);

      Assert.That(handler, Is.EqualTo(_handler));
      Assert.That(receivedArgs, Is.Not.Null);
      Assert.That(receivedArgs.BroadCastFlag, Is.EqualTo(65537));
    }
  }
}