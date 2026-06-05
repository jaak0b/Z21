using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.LocoNet;

namespace Z21.UnitTest.Core.ResponseHandler.LocoNet
{
  public class LocoNetResponseHandlerTest
  {
    [Test]
    public void Receive_CanHandleAndExtractsMessage()
    {
      LocoNetReceiveResponseHandler handler = new();
      byte[] response = [0x07, 0x00, 0xA0, 0x00, 0xB0, 0x01, 0x60];

      Assert.That(handler.CanHandle(response), Is.True);

      LocoNetMessageReceivedEventArgs? received = null;
      handler.OnLocoNetMessageReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.That(received, Is.Not.Null);
      Assert.That(received!.Message, Is.EqualTo(new byte[] { 0xB0, 0x01, 0x60 }));
    }

    [Test]
    public void Receive_RejectsOtherHeaders()
    {
      LocoNetReceiveResponseHandler handler = new();
      Assert.That(handler.CanHandle([0x07, 0x00, 0xA1, 0x00, 0xB0, 0x01, 0x60]), Is.False);
      Assert.That(handler.CanHandle([0x00]), Is.False);
    }

    [Test]
    public void Transmit_CanHandleAndExtractsMessage()
    {
      LocoNetTransmitResponseHandler handler = new();
      byte[] response = [0x07, 0x00, 0xA1, 0x00, 0xB0, 0x01, 0x60];

      Assert.That(handler.CanHandle(response), Is.True);

      LocoNetMessageReceivedEventArgs? received = null;
      handler.OnLocoNetMessageReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.That(received!.Message, Is.EqualTo(new byte[] { 0xB0, 0x01, 0x60 }));
    }

    [Test]
    public void FromLan_CanHandleAndExtractsMessage()
    {
      LocoNetFromLanResponseHandler handler = new();
      byte[] response = [0x07, 0x00, 0xA2, 0x00, 0xB0, 0x01, 0x60];

      Assert.That(handler.CanHandle(response), Is.True);

      LocoNetMessageReceivedEventArgs? received = null;
      handler.OnLocoNetMessageReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.That(received!.Message, Is.EqualTo(new byte[] { 0xB0, 0x01, 0x60 }));
    }

    [Test]
    public void DispatchAddress_DecodesAddressAndSlot()
    {
      LocoNetDispatchAddressResponseHandler handler = new();
      byte[] response = [0x07, 0x00, 0xA3, 0x00, 0x03, 0x00, 0x0B];

      Assert.That(handler.CanHandle(response), Is.True);

      LocoNetDispatchAddressReceivedEventArgs? received = null;
      handler.OnLocoNetDispatchAddressReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.Multiple(() =>
                      {
                        Assert.That(received!.LocoAddress, Is.EqualTo(3));
                        Assert.That(received.Slot, Is.EqualTo(11));
                      });
    }

    [Test]
    public void Detector_DecodesTypeAddressAndInfo()
    {
      LocoNetDetectorResponseHandler handler = new();
      byte[] response = [0x08, 0x00, 0xA4, 0x00, 0x01, 0xF8, 0x03, 0x01];

      Assert.That(handler.CanHandle(response), Is.True);

      LocoNetDetectorReceivedEventArgs? received = null;
      handler.OnLocoNetDetectorReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.Multiple(() =>
                      {
                        Assert.That(received!.Type, Is.EqualTo(0x01));
                        Assert.That(received.ReportAddress, Is.EqualTo(1016));
                        Assert.That(received.Info, Is.EqualTo(new byte[] { 0x01 }));
                      });
    }
  }
}
