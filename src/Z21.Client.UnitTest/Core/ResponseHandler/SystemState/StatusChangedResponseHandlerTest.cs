using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.SystemState;
using Z21.Core.ResponseParser;

namespace Z21.UnitTest.Core.ResponseHandler.SystemState
{
  public class StatusChangedResponseHandlerTest
  {
    private StatusChangedResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new(new CentralStateResponseParser());
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x00, 0x00, 0x40, 0x00, 0x62, 0x22, 0x01, 0x41];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x00, 0x00, 0x40, 0x00, 0x62, 0x22, 0x02, 0x41 }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    [TestCase(new byte[] { 0x00, 0x00, 0x40, 0x00, 0x62, 0x22, 0x00, 0x41 }, false, false, false, false)]
    [TestCase(new byte[] { 0x00, 0x00, 0x40, 0x00, 0x62, 0x22, 0x01, 0x41 }, true, false, false, false)]
    [TestCase(new byte[] { 0x00, 0x00, 0x40, 0x00, 0x62, 0x22, 0x02, 0x41 }, false, true, false, false)]
    [TestCase(new byte[] { 0x00, 0x00, 0x40, 0x00, 0x62, 0x22, 0x04, 0x41 }, false, false, true, false)]
    [TestCase(new byte[] { 0x00, 0x00, 0x40, 0x00, 0x62, 0x22, 0x20, 0x41 }, false, false, false, true)]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs(byte[] response, bool emergencyStop,
                                                                bool trackVoltageOff,
                                                                bool shortCircuit, bool programmingModeActive)
    {
      StatusChangedReceivedEventArgs? receivedArgs = null;
      StatusChangedResponseHandler? handler = null;
      _handler.OnStatusChangedReceived += (sender, args) =>
                                          {
                                            receivedArgs = args;
                                            handler = sender as StatusChangedResponseHandler;
                                          };

      _handler.Handle(response);

      Assert.Multiple(
                      () =>
                      {
                        Assert.That(handler, Is.EqualTo(_handler));
                        Assert.That(receivedArgs, Is.Not.Null);
                        Assert.That(receivedArgs?.CentralState, Is.Not.Null);
                      });
      Assert.Multiple(
                      () =>
                      {
                        Assert.That(receivedArgs.CentralState!.EmergencyStop, Is.EqualTo(emergencyStop));
                        Assert.That(receivedArgs.CentralState!.TrackVoltageOff, Is.EqualTo(trackVoltageOff));
                        Assert.That(receivedArgs.CentralState!.ShortCircuit, Is.EqualTo(shortCircuit));
                        Assert.That(receivedArgs.CentralState!.ProgrammingModeActive, Is.EqualTo(programmingModeActive));
                      });
    }
  }
}