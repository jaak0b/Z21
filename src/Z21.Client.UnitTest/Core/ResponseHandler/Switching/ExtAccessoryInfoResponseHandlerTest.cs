using Z21.Core.Codecs;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.Switching;

namespace Z21.UnitTest.Core.ResponseHandler.Switching
{
  public class ExtAccessoryInfoResponseHandlerTest
  {
    private ExtAccessoryInfoResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new(new AddressCodec());
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x0A, 0x00, 0x40, 0x00, 0x44, 0x00, 0x2F, 0x00, 0x00, 0x6B];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x0A, 0x00, 0x40, 0x01, 0x44, 0x00, 0x2F, 0x00, 0x00, 0x6B }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    [TestCase(0x00, 0x00, true)]
    [TestCase(0x06, 0x99, false)]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs(byte db2, byte db3, bool validData)
    {
      byte[] response = [0x0A, 0x00, 0x40, 0x00, 0x44, 0x00, 0x2F, db2, db3, 0x6B];

      ExtAccessoryInfoReceivedEventArgs? receivedArgs = null;
      ExtAccessoryInfoResponseHandler? handler = null;
      _handler.OnExtAccessoryInfoReceived += (sender, args) =>
                                             {
                                               receivedArgs = args;
                                               handler = sender as ExtAccessoryInfoResponseHandler;
                                             };

      _handler.Handle(response);

      Assert.That(handler, Is.EqualTo(_handler));
      Assert.That(receivedArgs, Is.Not.Null);
      Assert.That(receivedArgs.AccessoryAddress, Is.EqualTo(44));
      Assert.That(receivedArgs.EncodedState, Is.EqualTo(db2));
      Assert.That(receivedArgs.DataValid, Is.EqualTo(validData));
    }
  }
}