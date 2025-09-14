using Z21.Core.ResponseHandler.SystemState.TrackPower;

namespace Z21.UnitTest.Core.ResponseHandler.SystemState.TrackPower
{
  public class StoppedResponseHandlerTest
  {
    private StoppedResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x00, 0x00, 0x40, 0x00, 0x81, 0x00, 0x81];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x00, 0x00, 0x40, 0x00, 0x81, 0x01, 0x81 }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs()
    {
      byte[] response = [0x00, 0x00, 0x40, 0x00, 0x81, 0x00, 0x81];

      EventArgs? receivedArgs = null;
      StoppedResponseHandler? handler = null;
      _handler.OnStoppedReceived += (sender, args) =>
                                    {
                                      receivedArgs = args;
                                      handler = sender as StoppedResponseHandler;
                                    };

      _handler.Handle(response);

      Assert.Multiple(
                      () =>
                      {
                        Assert.That(handler, Is.EqualTo(_handler));
                        Assert.That(receivedArgs, Is.Not.Null);
                      });
    }
  }
}