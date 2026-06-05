using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.RailCom;
using Z21.Core.ResponseParser;

namespace Z21.UnitTest.Core.ResponseHandler.RailCom
{
  public class RailComDataChangedResponseHandlerTest
  {
    private RailComDataChangedResponseHandler _handler = null!;

    [SetUp]
    public void Setup() => _handler = new(new RailComDataParser());

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] valid = [0x11, 0x00, 0x88, 0x00, 0x03, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x05, 0x50, 0x0A, 0x00];
      Assert.That(_handler.CanHandle(valid), Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x11, 0x00, 0x80, 0x00, 0x03, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x05, 0x50, 0x0A, 0x00 }, TestName = "Wrong header")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response too small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      Assert.That(_handler.CanHandle(response), Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesParsedData()
    {
      byte[] response = [0x11, 0x00, 0x88, 0x00, 0x03, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x05, 0x50, 0x0A, 0x00];

      RailComDataReceivedEventArgs? received = null;
      _handler.OnRailComDataReceived += (_, args) => received = args;

      _handler.Handle(response);

      Assert.That(received, Is.Not.Null);
      Assert.Multiple(() =>
                      {
                        Assert.That(received!.Data.LocoAddress, Is.EqualTo(3));
                        Assert.That(received.Data.Speed, Is.EqualTo(80));
                      });
    }
  }
}
