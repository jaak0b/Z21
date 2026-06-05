using System.Text;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.ZLink;
using Z21.Core.ResponseParser;

namespace Z21.UnitTest.Core.ResponseHandler.ZLink
{
  public class ZLinkHardwareInfoResponseHandlerTest
  {
    private ZLinkHardwareInfoResponseHandler _handler = null!;

    [SetUp]
    public void Setup() => _handler = new(new ZLinkHardwareInfoParser());

    private static byte[] BuildFrame()
    {
      byte[] frame = new byte[63];
      frame[0] = 0x3F;
      frame[2] = 0xE8;
      frame[4] = 0x06;
      frame[5] = 0x91; frame[6] = 0x01; // HwID 401
      frame[7] = 0x01; // major
      frame[8] = 0x01; // minor
      frame[9] = 0x91; frame[10] = 0x0C; // build 3217
      Encoding.Latin1.GetBytes("EC FA BC", 0, 8, frame, 11); // MAC
      Encoding.Latin1.GetBytes("device", 0, 6, frame, 29); // Name
      return frame;
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      Assert.That(_handler.CanHandle(BuildFrame()), Is.True);
    }

    [Test]
    public void CanHandle_RejectsOtherFrames()
    {
      Assert.That(_handler.CanHandle([0x00]), Is.False);
      byte[] wrong = new byte[63];
      wrong[2] = 0xE8;
      wrong[4] = 0x07;
      Assert.That(_handler.CanHandle(wrong), Is.False);
    }

    [Test]
    public void Handle_NameWithoutTerminator_KeepsFullLength()
    {
      byte[] frame = BuildFrame();
      for (int i = 29; i < 62; i++)
        frame[i] = (byte)'X';

      ZLinkHardwareInfoReceivedEventArgs? received = null;
      _handler.OnZLinkHardwareInfoReceived += (_, args) => received = args;
      _handler.Handle(frame);

      Assert.That(received!.Info.Name, Has.Length.EqualTo(33));
    }

    [Test]
    public void Handle_NameStartingWithTerminator_IsEmpty()
    {
      byte[] frame = BuildFrame();
      for (int i = 29; i < 62; i++)
        frame[i] = 0x00;

      ZLinkHardwareInfoReceivedEventArgs? received = null;
      _handler.OnZLinkHardwareInfoReceived += (_, args) => received = args;
      _handler.Handle(frame);

      Assert.That(received!.Info.Name, Is.Empty);
    }

    [Test]
    public void Handle_DecodesAllFields()
    {
      ZLinkHardwareInfoReceivedEventArgs? received = null;
      _handler.OnZLinkHardwareInfoReceived += (_, args) => received = args;

      _handler.Handle(BuildFrame());

      Assert.That(received, Is.Not.Null);
      Assert.Multiple(() =>
                      {
                        Assert.That(received!.Info.HardwareId, Is.EqualTo(401));
                        Assert.That(received.Info.FirmwareMajor, Is.EqualTo(1));
                        Assert.That(received.Info.FirmwareMinor, Is.EqualTo(1));
                        Assert.That(received.Info.FirmwareBuild, Is.EqualTo(3217));
                        Assert.That(received.Info.MacAddress, Is.EqualTo("EC FA BC"));
                        Assert.That(received.Info.Name, Is.EqualTo("device"));
                      });
    }
  }
}
