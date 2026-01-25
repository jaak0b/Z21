using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.Switching;

namespace Z21.UnitTest.Core.ResponseHandler.Switching
{
  public class TurnoutInfoResponseHandlerTest
  {
    private TurnoutInfoResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x09, 0x00, 0x40, 0x00, 0x43, 0x00, 0x0E, 0x01, 0x4C];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x09, 0x00, 0x41, 0x00, 0x43, 0x00, 0x0E, 0x01, 0x4C }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    [TestCase(0x00, null)]
    [TestCase(0x03, null)]
    [TestCase(0x01, AccessoryOutput.Output1)]
    [TestCase(0x02, AccessoryOutput.Output2)]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs(byte db2, AccessoryOutput? accessoryOutput)
    {
      byte[] response = [0x09, 0x00, 0x40, 0x00, 0x43, 0x00, 0x40, db2, 0x01];

      TurnoutInfoReceivedEventArgs? receivedArgs = null;
      TurnoutInfoResponseHandler? handler = null;
      _handler.OnTurnoutInfoReceived += (sender, args) =>
                                        {
                                          receivedArgs = args;
                                          handler = sender as TurnoutInfoResponseHandler;
                                        };

      _handler.Handle(response);

      Assert.That(handler, Is.EqualTo(_handler));
      Assert.That(receivedArgs, Is.Not.Null);
      Assert.That(receivedArgs.AccessoryAddress, Is.EqualTo(65));
      Assert.That(receivedArgs.AccessoryOutput, Is.EqualTo(accessoryOutput));
    }
  }
}