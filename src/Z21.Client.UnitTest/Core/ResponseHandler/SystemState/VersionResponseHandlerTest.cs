using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.SystemState;

namespace Z21.UnitTest.Core.ResponseHandler.SystemState
{
  public class VersionResponseHandlerTest
  {
    private VersionResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x00, 0x00, 0x40, 0x00, 0x63, 0x21, 0x01, 0x02];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x00, 0x00, 0x41, 0x00, 0x62, 0x20, 0x01, 0x02 }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs()
    {
      byte[] response = [0x00, 0x00, 0x40, 0x00, 0x63, 0x21, 0x43, 0xBB];

      VersionReceivedEventArgs? receivedArgs = null;
      VersionResponseHandler? handler = null;
      _handler.OnVersionReceived += (sender, args) =>
                                    {
                                      receivedArgs = args;
                                      handler = sender as VersionResponseHandler;
                                    };

      _handler.Handle(response);

      Assert.Multiple(() =>
                      {
                        Assert.That(handler, Is.EqualTo(_handler));
                        Assert.That(receivedArgs, Is.Not.Null);
                      });
      Assert.Multiple(() =>
                      {
                        Assert.That(receivedArgs.FirmwareVersion, Is.Not.Null);
                        Assert.That(receivedArgs.FirmwareVersion, Is.EqualTo(new FirmwareVersion(4, 3)));
                        Assert.That(receivedArgs.CommandStationId, Is.EqualTo(0xBB));
                      });
    }
  }
}