using Z21.Core.Command.Programming;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Programming
{
  public class DccReadRegisterCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((byte)0x01, new byte[] { 0x08, 0x00, 0x40, 0x00, 0x22, 0x11, 0x01, 0x32 })]
    public void Ctor_SetsCorrectDataBits(byte register, byte[] expected)
    {
      DccReadRegisterCommand command = Factory.Create<DccReadRegisterCommand>(register);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
