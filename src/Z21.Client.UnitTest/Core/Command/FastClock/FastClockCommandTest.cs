using CommandStation.Model;
using Z21.Core.Command.FastClock;
using Z21.Core.Model;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.FastClock
{
  public class FastClockCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase(FastClockAction.Read, new byte[] { 0x07, 0x00, 0xCC, 0x00, 0x21, 0x2A, 0x0B })]
    [TestCase(FastClockAction.Start, new byte[] { 0x07, 0x00, 0xCC, 0x00, 0x21, 0x2C, 0x0D })]
    [TestCase(FastClockAction.Stop, new byte[] { 0x07, 0x00, 0xCC, 0x00, 0x21, 0x2D, 0x0C })]
    public void Control_Action_BuildsChecksummedFrame(FastClockAction action, byte[] expected)
    {
      FastClockControlCommand command = Factory.Create<FastClockControlCommand>(action);
      Assert.That(command.Data, Is.EqualTo(expected));
    }

    [Test]
    public void Control_SetModelTime_EncodesDayHourMinuteRate()
    {
      FastClockControlCommand command = Factory.Create<FastClockControlCommand>(new ModelTime(0, 12, 30, 0, 8));
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x0A, 0x00, 0xCC, 0x00, 0x24, 0x2B, 0x0C, 0x1E, 0x08, 0x15 }));
    }

    [Test]
    public void Control_SetModelTime_EncodesDayInHighBits()
    {
      FastClockControlCommand command = Factory.Create<FastClockControlCommand>(new ModelTime(2, 12, 30, 0, 8));
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x0A, 0x00, 0xCC, 0x00, 0x24, 0x2B, 0x4C, 0x1E, 0x08, 0x55 }));
    }

    [Test]
    public void GetSettings_BuildsRequest()
    {
      GetFastClockSettingsCommand command = Factory.Create<GetFastClockSettingsCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x05, 0x00, 0xCE, 0x00, 0x04 }));
    }

    [Test]
    public void SetSettings_SettingsOnly()
    {
      SetFastClockSettingsCommand command = Factory.Create<SetFastClockSettingsCommand>((byte)0x4F);
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x05, 0x00, 0xCF, 0x00, 0x4F }));
    }

    [Test]
    public void SetSettings_SettingsAndRate()
    {
      SetFastClockSettingsWithRateCommand command = Factory.Create<SetFastClockSettingsWithRateCommand>((byte)0x4F, (byte)0x01);
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x06, 0x00, 0xCF, 0x00, 0x4F, 0x01 }));
    }

    [Test]
    public void SetSettings_SettingsRateAndStart()
    {
      SetFastClockSettingsWithStartTimeCommand command = Factory.Create<SetFastClockSettingsWithStartTimeCommand>((byte)0x4F, (byte)0x01, (byte)0x0C, (byte)0x1E);
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x08, 0x00, 0xCF, 0x00, 0x4F, 0x01, 0x0C, 0x1E }));
    }
  }
}
