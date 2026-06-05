using Z21.Core.Command.Programming;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Programming
{
  public class DccWriteRegisterCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((byte)0x01, (byte)0x05, new byte[] { 0x09, 0x00, 0x40, 0x00, 0x23, 0x12, 0x01, 0x05, 0x35 })]
    public void Ctor_SetsCorrectDataBits(byte register, byte value, byte[] expected)
    {
      DccWriteRegisterCommand command = Factory.Create<DccWriteRegisterCommand>(register, value);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
