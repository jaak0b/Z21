using Z21.Core.Command.Driving;
using Z21.Core.Exception;
using Z21.Core.Model;

namespace Z21.UnitTest.Core.Command.Driving
{
  public class SetLocoDriveCommandTest
  {
    [Test]
    [TestCase(DccSpeedMode.Steps14, (ushort)15)]
    [TestCase(DccSpeedMode.Steps28, (ushort)29)]
    [TestCase(DccSpeedMode.Steps128, (ushort)127)]
    public void Ctor_SpeedOutOfRange_ThrowsLocoSpeedOutOfRangeException(DccSpeedMode dccSpeedMode, ushort locoSpeed)
    {
      Assert.Throws<LocoSpeedOutOfRangeException>(() => _ = new SetLocoDriveCommand(dccSpeedMode, 0, DrivingDirection.Forward, locoSpeed));
    }

    [Test]
    [TestCase(DccSpeedMode.Steps128, (ushort)20, DrivingDirection.Forward, (ushort)1, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0x13, 0x00, 0x14, 0x82, 0x61 })]
    [TestCase(DccSpeedMode.Steps28, (ushort)101, DrivingDirection.Backward, (ushort)2, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0x12, 0x00, 0x65, 0x12, 0x81 })]
    [TestCase(DccSpeedMode.Steps28, (ushort)101, DrivingDirection.Backward, (ushort)3, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0x12, 0x00, 0x65, 0x03, 0x90 })]
    [TestCase(DccSpeedMode.Steps14, (ushort)130, DrivingDirection.Backward, (ushort)1, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0x10, 0xC0, 0x82, 0x2, 0xB4 })]
    public void Ctor_SetsCorrectDataBits(DccSpeedMode dccSpeedMode, ushort locoAddress, DrivingDirection drivingDirection, ushort locoSpeed, byte[] data)
    {
      SetLocoDriveCommand command = new(dccSpeedMode, locoAddress, drivingDirection, locoSpeed);
      Assert.That(command.Data, Is.EqualTo(data));
    }
  }
}