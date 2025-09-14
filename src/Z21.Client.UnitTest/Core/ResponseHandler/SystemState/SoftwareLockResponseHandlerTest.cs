using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.SystemState;

namespace Z21.UnitTest.Core.ResponseHandler.SystemState
{
  //response from z21: 05-00-18-00-02
  public class SoftwareLockResponseHandlerTest
  {
    private SoftwareLockResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x05, 0x00, 0x18, 0x00, 0x02];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x05, 0x00, 0x17, 0x00, 0x02 }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs()
    {
      byte[] response = [0x05, 0x00, 0x18, 0x00, 0x02];

      SoftwareLockReceivedEventArgs? receivedArgs = null;
      SoftwareLockResponseHandler? handler = null;
      _handler.OnSoftwareLockReceived += (sender, args) =>
                                         {
                                           receivedArgs = args;
                                           handler = sender as SoftwareLockResponseHandler;
                                         };

      _handler.Handle(response);

      Assert.That(handler, Is.EqualTo(_handler));
      Assert.That(receivedArgs, Is.Not.Null);
      Assert.That(receivedArgs!.Code, Is.EqualTo(Z21SoftwareLock.Z21StartUnlocked));
    }
  }
}