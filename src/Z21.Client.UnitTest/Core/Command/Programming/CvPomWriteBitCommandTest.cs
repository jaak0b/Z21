using Z21.Core.Command.Programming;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Programming
{
  public class CvPomWriteBitCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)3, (ushort)0, (byte)2, true, new byte[] { 0x0C, 0x00, 0x40, 0x00, 0xE6, 0x30, 0x00, 0x03, 0xE8, 0x00, 0xFA, 0xC7 })]
    [TestCase((ushort)3, (ushort)0, (byte)2, false, new byte[] { 0x0C, 0x00, 0x40, 0x00, 0xE6, 0x30, 0x00, 0x03, 0xE8, 0x00, 0xF2, 0xCF })]
    [TestCase((ushort)3, (ushort)256, (byte)2, true, new byte[] { 0x0C, 0x00, 0x40, 0x00, 0xE6, 0x30, 0x00, 0x03, 0xE9, 0x00, 0xFA, 0xC6 })]
    public void Ctor_SetsCorrectDataBits(ushort locoAddress, ushort cvAddress, byte bitPosition, bool bitValue, byte[] expected)
    {
      CvPomWriteBitCommand command = Factory.Create<CvPomWriteBitCommand>(locoAddress, cvAddress, bitPosition, bitValue);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
