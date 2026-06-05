using Z21.Core.Codecs;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.Programming;

namespace Z21.UnitTest.Core.ResponseHandler.Programming
{
  public class CvResultResponseHandlerTest
  {
    private CvResultResponseHandler _handler = null!;

    [SetUp]
    public void Setup() => _handler = new(new AddressCodec());

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x0A, 0x00, 0x40, 0x00, 0x64, 0x14, 0x00, 0x1C, 0x05, 0x00];
      Assert.That(_handler.CanHandle(validResponse), Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x0A, 0x00, 0x40, 0x00, 0x64, 0x13, 0x00, 0x1C, 0x05, 0x00 }, TestName = "Wrong DB0")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response too small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      Assert.That(_handler.CanHandle(response), Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEventWithCvAndValue()
    {
      byte[] response = [0x0A, 0x00, 0x40, 0x00, 0x64, 0x14, 0x00, 0x1C, 0x05, 0x00];

      CvResultReceivedEventArgs? received = null;
      _handler.OnCvResultReceived += (_, args) => received = args;

      _handler.Handle(response);

      Assert.That(received, Is.Not.Null);
      Assert.Multiple(() =>
                      {
                        Assert.That(received!.CvAddress, Is.EqualTo(28));
                        Assert.That(received.Value, Is.EqualTo(5));
                      });
    }
  }
}
