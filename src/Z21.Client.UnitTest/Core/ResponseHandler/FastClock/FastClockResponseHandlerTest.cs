using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.FastClock;

namespace Z21.UnitTest.Core.ResponseHandler.FastClock
{
  public class FastClockResponseHandlerTest
  {
    [Test]
    public void Data_DecodesTimeAndFlags()
    {
      FastClockDataResponseHandler handler = new();
      // day=0, hour=12 (0x0C), minute=30 (0x1E), second=45 (0x2D), rate=8, settings=0x80
      byte[] response = [0x0C, 0x00, 0xCD, 0x00, 0x66, 0x25, 0x0C, 0x1E, 0x2D, 0x08, 0x80, 0x00];

      Assert.That(handler.CanHandle(response), Is.True);

      FastClockDataReceivedEventArgs? received = null;
      handler.OnFastClockDataReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.That(received, Is.Not.Null);
      Assert.Multiple(() =>
                      {
                        Assert.That(received!.Data.Day, Is.EqualTo(0));
                        Assert.That(received.Data.Hour, Is.EqualTo(12));
                        Assert.That(received.Data.Minute, Is.EqualTo(30));
                        Assert.That(received.Data.Second, Is.EqualTo(45));
                        Assert.That(received.Data.Rate, Is.EqualTo(8));
                        Assert.That(received.Data.IsStopped, Is.False);
                        Assert.That(received.Data.IsHalted, Is.False);
                        Assert.That(received.Data.Settings, Is.EqualTo(FastClockSettings.Enabled));
                      });
    }

    [Test]
    public void Data_DayInHighBits_IsDecoded()
    {
      FastClockDataResponseHandler handler = new();
      // dayHour = 0x4C => day 2, hour 12
      byte[] response = [0x0C, 0x00, 0xCD, 0x00, 0x66, 0x25, 0x4C, 0x1E, 0x2D, 0x08, 0x80, 0x00];

      FastClockDataReceivedEventArgs? received = null;
      handler.OnFastClockDataReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.Multiple(() =>
                      {
                        Assert.That(received!.Data.Day, Is.EqualTo(2));
                        Assert.That(received.Data.Hour, Is.EqualTo(12));
                      });
    }

    [Test]
    public void Data_StopAndHaltFlags_AreDecoded()
    {
      FastClockDataResponseHandler handler = new();
      byte[] response = [0x0C, 0x00, 0xCD, 0x00, 0x66, 0x25, 0x0C, 0x1E, 0xC5, 0x08, 0x80, 0x00];

      FastClockDataReceivedEventArgs? received = null;
      handler.OnFastClockDataReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.Multiple(() =>
                      {
                        Assert.That(received!.Data.Second, Is.EqualTo(5));
                        Assert.That(received.Data.IsStopped, Is.True);
                        Assert.That(received.Data.IsHalted, Is.True);
                      });
    }

    [Test]
    public void Data_RejectsOtherHeaders()
    {
      FastClockDataResponseHandler handler = new();
      Assert.That(handler.CanHandle([0x0C, 0x00, 0xCE, 0x00, 0x66, 0x25, 0x0C, 0x1E, 0x2D, 0x08, 0x80, 0x00]), Is.False);
      Assert.That(handler.CanHandle([0x00]), Is.False);
    }

    [Test]
    public void Settings_DecodesFields()
    {
      FastClockSettingsResponseHandler handler = new();
      byte[] response = [0x08, 0x00, 0xCE, 0x00, 0x4F, 0x01, 0x0C, 0x1E];

      Assert.That(handler.CanHandle(response), Is.True);

      FastClockSettingsReceivedEventArgs? received = null;
      handler.OnFastClockSettingsReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.Multiple(() =>
                      {
                        Assert.That(received!.Settings.Settings, Is.EqualTo((FastClockSettings)0x4F));
                        Assert.That(received.Settings.Rate, Is.EqualTo(1));
                        Assert.That(received.Settings.StartDayHour, Is.EqualTo(0x0C));
                        Assert.That(received.Settings.StartMinute, Is.EqualTo(0x1E));
                      });
    }
  }
}
