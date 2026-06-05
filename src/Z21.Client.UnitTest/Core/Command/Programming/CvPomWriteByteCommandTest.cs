using Z21.Core.Command.Programming;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Programming
{
  public class CvPomWriteByteCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)3, (ushort)0, (byte)0x05, new byte[] { 0x0C, 0x00, 0x40, 0x00, 0xE6, 0x30, 0x00, 0x03, 0xEC, 0x00, 0x05, 0x3C })]
    [TestCase((ushort)3, (ushort)256, (byte)0x05, new byte[] { 0x0C, 0x00, 0x40, 0x00, 0xE6, 0x30, 0x00, 0x03, 0xED, 0x00, 0x05, 0x3D })]
    public void Ctor_SetsCorrectDataBits(ushort locoAddress, ushort cvAddress, byte value, byte[] expected)
    {
      CvPomWriteByteCommand command = Factory.Create<CvPomWriteByteCommand>(locoAddress, cvAddress, value);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
