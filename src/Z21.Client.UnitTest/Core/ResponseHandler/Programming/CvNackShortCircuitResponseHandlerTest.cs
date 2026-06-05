using Z21.Core.ResponseHandler.Programming;

namespace Z21.UnitTest.Core.ResponseHandler.Programming
{
  public class CvNackShortCircuitResponseHandlerTest
  {
    private CvNackShortCircuitResponseHandler _handler = null!;

    [SetUp]
    public void Setup() => _handler = new();

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x07, 0x00, 0x40, 0x00, 0x61, 0x12, 0x73];
      Assert.That(_handler.CanHandle(validResponse), Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x07, 0x00, 0x40, 0x00, 0x61, 0x13, 0x72 }, TestName = "Plain nack")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response too small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      Assert.That(_handler.CanHandle(response), Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEvent()
    {
      byte[] response = [0x07, 0x00, 0x40, 0x00, 0x61, 0x12, 0x73];

      bool raised = false;
      _handler.OnCvNackShortCircuitReceived += (_, _) => raised = true;

      _handler.Handle(response);

      Assert.That(raised, Is.True);
    }
  }
}
