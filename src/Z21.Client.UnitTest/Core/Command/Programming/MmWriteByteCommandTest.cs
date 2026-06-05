using Z21.Core.Command.Programming;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Programming
{
  public class MmWriteByteCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((byte)0x00, (byte)0x05, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0x24, 0xFF, 0x00, 0x00, 0x05, 0xDE })]
    public void Ctor_MatchesSpecExample(byte register, byte value, byte[] expected)
    {
      MmWriteByteCommand command = Factory.Create<MmWriteByteCommand>(register, value);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
