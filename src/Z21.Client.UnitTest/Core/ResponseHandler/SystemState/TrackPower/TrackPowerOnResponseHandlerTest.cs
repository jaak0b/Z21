using Z21.Core.ResponseHandler.SystemState.TrackPower;

namespace Z21.UnitTest.Core.ResponseHandler.SystemState.TrackPower
{
  public class TrackPowerOnResponseHandlerTest
  {
    private TrackPowerOnResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x00, 0x00, 0x40, 0x00, 0x61, 0x01, 0x60];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x00, 0x00, 0x40, 0x00, 0x62, 0x01, 0x60 }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs()
    {
      byte[] response = [0x00, 0x00, 0x40, 0x00, 0x61, 0x01, 0x60];

      EventArgs? receivedArgs = null;
      TrackPowerOnResponseHandler? handler = null;
      _handler.OnTrackPowerOnReceived += (sender, args) =>
                                         {
                                           receivedArgs = args;
                                           handler = sender as TrackPowerOnResponseHandler;
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