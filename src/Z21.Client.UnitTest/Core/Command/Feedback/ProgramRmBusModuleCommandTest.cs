using Z21.Core.Command.Feedback;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Feedback
{
  public class ProgramRmBusModuleCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((byte)5, new byte[] { 0x05, 0x00, 0x82, 0x00, 0x05 })]
    [TestCase((byte)0, new byte[] { 0x05, 0x00, 0x82, 0x00, 0x00 })]
    public void Ctor_SetsCorrectDataBits(byte address, byte[] expected)
    {
      ProgramRmBusModuleCommand command = Factory.Create<ProgramRmBusModuleCommand>(address);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
