using Z21.Core.Command.Programming;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Programming
{
  public class CvWriteCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)0, (byte)0x03, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0x24, 0x12, 0x00, 0x00, 0x03, 0x35 })]
    public void Ctor_SetsCorrectDataBits(ushort cvAddress, byte value, byte[] expected)
    {
      CvWriteCommand command = Factory.Create<CvWriteCommand>(cvAddress, value);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
