using System;
using Z21.Core.Command.Driving;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Driving
{
  public class SetLocoBinaryStateCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)3, (ushort)29, true, new byte[] { 0x0B, 0x00, 0x40, 0x00, 0xE5, 0x5F, 0x00, 0x03, 0x9D, 0x00, 0x24 })]
    [TestCase((ushort)1000, (ushort)32767, false, new byte[] { 0x0B, 0x00, 0x40, 0x00, 0xE5, 0x5F, 0xC3, 0xE8, 0x7F, 0xFF, 0x11 })]
    public void Ctor_SetsCorrectDataBits(ushort locoAddress, ushort binaryStateAddress, bool enabled, byte[] expected)
    {
      SetLocoBinaryStateCommand command = Factory.Create<SetLocoBinaryStateCommand>(locoAddress, binaryStateAddress, enabled);
      Assert.That(command.Data, Is.EqualTo(expected));
    }

    [Test]
    [TestCase((ushort)28)]
    [TestCase((ushort)0)]
    [TestCase((ushort)32768)]
    public void Ctor_BinaryStateAddressOutOfRange_ThrowsWithMessage(ushort binaryStateAddress)
    {
      ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => Factory.Create<SetLocoBinaryStateCommand>((ushort)3, binaryStateAddress, true))!;
      Assert.That(exception.Message, Does.Contain("between 29 and 32767"));
    }
  }
}
