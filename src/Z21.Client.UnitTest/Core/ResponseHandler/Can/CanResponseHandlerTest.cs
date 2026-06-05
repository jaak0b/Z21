using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.Can;

namespace Z21.UnitTest.Core.ResponseHandler.Can
{
  public class CanResponseHandlerTest
  {
    [Test]
    public void Detector_DecodesAllFields()
    {
      CanDetectorResponseHandler handler = new();
      byte[] response = [0x0E, 0x00, 0xC4, 0x00, 0x01, 0xC1, 0x05, 0x00, 0x03, 0x01, 0x00, 0x11, 0x00, 0x00];

      Assert.That(handler.CanHandle(response), Is.True);

      CanDetectorReceivedEventArgs? received = null;
      handler.OnCanDetectorReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.That(received, Is.Not.Null);
      Assert.Multiple(() =>
                      {
                        Assert.That(received!.Data.NetworkId, Is.EqualTo(0xC101));
                        Assert.That(received.Data.ModuleAddress, Is.EqualTo(5));
                        Assert.That(received.Data.Port, Is.EqualTo(3));
                        Assert.That(received.Data.Type, Is.EqualTo(0x01));
                        Assert.That(received.Data.Value1, Is.EqualTo(0x1100));
                        Assert.That(received.Data.Value2, Is.EqualTo(0));
                      });
    }

    [Test]
    public void Detector_RejectsOtherHeaders()
    {
      CanDetectorResponseHandler handler = new();
      Assert.That(handler.CanHandle([0x0E, 0x00, 0xC8, 0x00, 0x01, 0xC1, 0x05, 0x00, 0x03, 0x01, 0x00, 0x11, 0x00, 0x00]), Is.False);
      Assert.That(handler.CanHandle([0x00]), Is.False);
    }

    [Test]
    public void DeviceDescription_DecodesNetworkIdAndName()
    {
      CanDeviceDescriptionResponseHandler handler = new();
      byte[] response = [0x16, 0x00, 0xC8, 0x00, 0x01, 0xC1, 0x41, 0x42, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

      Assert.That(handler.CanHandle(response), Is.True);

      CanDeviceDescriptionReceivedEventArgs? received = null;
      handler.OnCanDeviceDescriptionReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.Multiple(() =>
                      {
                        Assert.That(received!.NetworkId, Is.EqualTo(0xC101));
                        Assert.That(received.Name, Is.EqualTo("AB"));
                      });
    }

    [Test]
    public void DeviceDescription_NameWithoutTerminator_KeepsFullLength()
    {
      CanDeviceDescriptionResponseHandler handler = new();
      byte[] response = new byte[22];
      response[2] = 0xC8;
      for (int i = 6; i < 22; i++)
        response[i] = (byte)'X';

      CanDeviceDescriptionReceivedEventArgs? received = null;
      handler.OnCanDeviceDescriptionReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.That(received!.Name, Has.Length.EqualTo(16));
    }

    [Test]
    public void DeviceDescription_NameStartingWithTerminator_IsEmpty()
    {
      CanDeviceDescriptionResponseHandler handler = new();
      byte[] response = new byte[22];
      response[2] = 0xC8;

      CanDeviceDescriptionReceivedEventArgs? received = null;
      handler.OnCanDeviceDescriptionReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.That(received!.Name, Is.Empty);
    }

    [Test]
    public void BoosterSystemState_DecodesAllFields()
    {
      CanBoosterSystemStateResponseHandler handler = new();
      byte[] response = [0x0E, 0x00, 0xCA, 0x00, 0x01, 0xC1, 0x01, 0x00, 0x80, 0x00, 0x10, 0x27, 0xE8, 0x03];

      Assert.That(handler.CanHandle(response), Is.True);

      CanBoosterSystemStateReceivedEventArgs? received = null;
      handler.OnCanBoosterSystemStateReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.Multiple(() =>
                      {
                        Assert.That(received!.State.NetworkId, Is.EqualTo(0xC101));
                        Assert.That(received.State.OutputPort, Is.EqualTo(1));
                        Assert.That(received.State.State, Is.EqualTo(CanBoosterState.TrackVoltageOff));
                        Assert.That(received.State.VccVoltage, Is.EqualTo(10000));
                        Assert.That(received.State.Current, Is.EqualTo(1000));
                      });
    }
  }
}
