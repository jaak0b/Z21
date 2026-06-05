using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.Decoder;
using Z21.Core.ResponseParser;

namespace Z21.UnitTest.Core.ResponseHandler.Decoder
{
  public class DecoderSystemStateResponseHandlerTest
  {
    private DecoderSystemStateResponseHandler _handler = null!;

    [SetUp]
    public void Setup() => _handler = new(new SwitchDecoderSystemStateParser(), new SignalDecoderSystemStateParser());

    private static byte[] SwitchFrame()
    {
      byte[] frame = new byte[48];
      frame[0] = 0x30;
      frame[2] = 0xDA;
      // payload begins at index 4
      frame[4] = 0x64; // Current = 100
      frame[8] = 0xE4; frame[9] = 0x0C; // Voltage = 3300
      frame[10] = 0x02; // CentralState
      frame[11] = 0x20; // CentralStateEx
      frame[12] = 0x11; // OutputStates[0]
      frame[36] = 0x01; // Address = 1
      frame[38] = 0x02; // Address2 = 2
      frame[46] = 0x03; // Dimmed
      return frame;
    }

    private static byte[] SignalFrame()
    {
      byte[] frame = new byte[46];
      frame[0] = 0x2E;
      frame[2] = 0xDA;
      frame[8] = 0xE0; frame[9] = 0x2E; // Voltage = 12000
      frame[10] = 0x01; // CentralState
      frame[12] = 0xAB; // OutputStates[0]
      frame[16] = 0x10; // SignalDccExt[0]
      frame[27] = 0x02; // SignalCount
      frame[28] = 0x05; // SignalConfig[0]
      frame[36] = 0x10; // Address = 16
      return frame;
    }

    [Test]
    public void Handle_SwitchDecoderFrame_RaisesSwitchState()
    {
      byte[] frame = SwitchFrame();
      Assert.That(_handler.CanHandle(frame), Is.True);

      SwitchDecoderSystemStateReceivedEventArgs? received = null;
      _handler.OnSwitchDecoderSystemStateReceived += (_, args) => received = args;
      _handler.Handle(frame);

      Assert.That(received, Is.Not.Null);
      Assert.Multiple(() =>
                      {
                        Assert.That(received!.State.Current, Is.EqualTo(100));
                        Assert.That(received.State.Voltage, Is.EqualTo(3300));
                        Assert.That(received.State.CentralState, Is.EqualTo(0x02));
                        Assert.That(received.State.OutputStates[0], Is.EqualTo(0x11));
                        Assert.That(received.State.Address, Is.EqualTo(1));
                        Assert.That(received.State.Address2, Is.EqualTo(2));
                        Assert.That(received.State.Dimmed, Is.EqualTo(0x03));
                      });
    }

    [Test]
    public void Handle_SignalDecoderFrame_RaisesSignalState()
    {
      byte[] frame = SignalFrame();
      Assert.That(_handler.CanHandle(frame), Is.True);

      SignalDecoderSystemStateReceivedEventArgs? received = null;
      _handler.OnSignalDecoderSystemStateReceived += (_, args) => received = args;
      _handler.Handle(frame);

      Assert.That(received, Is.Not.Null);
      Assert.Multiple(() =>
                      {
                        Assert.That(received!.State.Voltage, Is.EqualTo(12000));
                        Assert.That(received.State.CentralState, Is.EqualTo(0x01));
                        Assert.That(received.State.OutputStates[0], Is.EqualTo(0xAB));
                        Assert.That(received.State.SignalDccExt[0], Is.EqualTo(0x10));
                        Assert.That(received.State.SignalCount, Is.EqualTo(2));
                        Assert.That(received.State.SignalConfig[0], Is.EqualTo(0x05));
                        Assert.That(received.State.Address, Is.EqualTo(16));
                      });
    }

    [Test]
    public void CanHandle_RejectsOtherHeaders()
    {
      Assert.That(_handler.CanHandle([0x00]), Is.False);
      byte[] wrongHeader = new byte[48];
      wrongHeader[2] = 0xDB;
      Assert.That(_handler.CanHandle(wrongHeader), Is.False);
    }
  }
}
