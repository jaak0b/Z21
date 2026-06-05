using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.Feedback;

namespace Z21.UnitTest.Core.ResponseHandler.Feedback
{
  public class RmBusDataChangedResponseHandlerTest
  {
    private RmBusDataChangedResponseHandler _handler = null!;

    [SetUp]
    public void Setup() => _handler = new();

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x0F, 0x00, 0x80, 0x00, 0x01, 0x01, 0x00, 0xC5, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
      Assert.That(_handler.CanHandle(validResponse), Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x0F, 0x00, 0x81, 0x00, 0x01, 0x01, 0x00, 0xC5, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, TestName = "Wrong header")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response too small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      Assert.That(_handler.CanHandle(response), Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesGroupIndexAndStates()
    {
      byte[] response = [0x0F, 0x00, 0x80, 0x00, 0x01, 0x01, 0x00, 0xC5, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

      RmBusDataReceivedEventArgs? received = null;
      _handler.OnRmBusDataReceived += (_, args) => received = args;

      _handler.Handle(response);

      Assert.That(received, Is.Not.Null);
      Assert.Multiple(() =>
                      {
                        Assert.That(received!.GroupIndex, Is.EqualTo(1));
                        Assert.That(received.FeedbackStates, Is.EqualTo(new byte[] { 0x01, 0x00, 0xC5, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                      });
    }
  }
}
