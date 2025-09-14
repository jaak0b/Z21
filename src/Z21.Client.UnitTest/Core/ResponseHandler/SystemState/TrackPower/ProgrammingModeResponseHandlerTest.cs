using Z21.Core.ResponseHandler.SystemState.TrackPower;

namespace Z21.UnitTest.Core.ResponseHandler.SystemState.TrackPower
{
  public class ProgrammingModeResponseHandlerTest
  {
    private ProgrammingModeResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x00, 0x00, 0x40, 0x00, 0x61, 0x02, 0x63];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x00, 0x00, 0x40, 0x00, 0x61, 0x03, 0x63 }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs()
    {
      byte[] response = [0x00, 0x00, 0x40, 0x00, 0x61, 0x02, 0x63];

      EventArgs? receivedArgs = null;
      ProgrammingModeResponseHandler? handler = null;
      _handler.OnProgrammingModeReceived += (sender, args) =>
                                            {
                                              receivedArgs = args;
                                              handler = sender as ProgrammingModeResponseHandler;
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