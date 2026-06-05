using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.Decoder;

namespace Z21.UnitTest.Core.ResponseHandler.Decoder
{
  public class DecoderDescriptionResponseHandlerTest
  {
    private DecoderDescriptionResponseHandler _handler = null!;

    [SetUp]
    public void Setup() => _handler = new();

    [Test]
    public void Handle_DecodesName()
    {
      byte[] response = new byte[36];
      response[0] = 0x24;
      response[2] = 0xD8;
      response[4] = 0x41;
      response[5] = 0x42;

      Assert.That(_handler.CanHandle(response), Is.True);

      DecoderDescriptionReceivedEventArgs? received = null;
      _handler.OnDecoderDescriptionReceived += (_, args) => received = args;
      _handler.Handle(response);

      Assert.That(received!.Name, Is.EqualTo("AB"));
    }

    [Test]
    public void Handle_NameWithoutTerminator_KeepsFullLength()
    {
      byte[] response = new byte[36];
      response[2] = 0xD8;
      for (int i = 4; i < 36; i++)
        response[i] = (byte)'X';

      DecoderDescriptionReceivedEventArgs? received = null;
      _handler.OnDecoderDescriptionReceived += (_, args) => received = args;
      _handler.Handle(response);

      Assert.That(received!.Name, Has.Length.EqualTo(32));
    }

    [Test]
    public void Handle_NameStartingWithTerminator_IsEmpty()
    {
      byte[] response = new byte[36];
      response[2] = 0xD8;

      DecoderDescriptionReceivedEventArgs? received = null;
      _handler.OnDecoderDescriptionReceived += (_, args) => received = args;
      _handler.Handle(response);

      Assert.That(received!.Name, Is.Empty);
    }

    [Test]
    public void CanHandle_RejectsOtherHeaders()
    {
      Assert.That(_handler.CanHandle([0x00]), Is.False);
      byte[] wrong = new byte[36];
      wrong[2] = 0xDA;
      Assert.That(_handler.CanHandle(wrong), Is.False);
    }
  }
}
