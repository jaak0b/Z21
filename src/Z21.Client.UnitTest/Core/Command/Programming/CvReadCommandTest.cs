using Z21.Core.Command.Programming;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Programming
{
  public class CvReadCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)0, new byte[] { 0x09, 0x00, 0x40, 0x00, 0x23, 0x11, 0x00, 0x00, 0x32 })]
    [TestCase((ushort)28, new byte[] { 0x09, 0x00, 0x40, 0x00, 0x23, 0x11, 0x00, 0x1C, 0x2E })]
    public void Ctor_SetsCorrectDataBits(ushort cvAddress, byte[] expected)
    {
      CvReadCommand command = Factory.Create<CvReadCommand>(cvAddress);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
