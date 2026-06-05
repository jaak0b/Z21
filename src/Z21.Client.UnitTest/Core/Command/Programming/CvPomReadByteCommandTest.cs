using Z21.Core.Command.Programming;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Programming
{
  public class CvPomReadByteCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)3, (ushort)0, new byte[] { 0x0C, 0x00, 0x40, 0x00, 0xE6, 0x30, 0x00, 0x03, 0xE4, 0x00, 0x00, 0x31 })]
    public void Ctor_SetsCorrectDataBits(ushort locoAddress, ushort cvAddress, byte[] expected)
    {
      CvPomReadByteCommand command = Factory.Create<CvPomReadByteCommand>(locoAddress, cvAddress);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
