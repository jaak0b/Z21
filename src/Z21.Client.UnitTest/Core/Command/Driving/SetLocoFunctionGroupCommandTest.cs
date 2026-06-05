using Z21.Core.Command.Driving;
using Z21.Core.Model;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Driving
{
  public class SetLocoFunctionGroupCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)3, LocoFunctionGroup.Group1, (byte)0x10, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0x20, 0x00, 0x03, 0x10, 0xD7 })]
    [TestCase((ushort)200, LocoFunctionGroup.Group4, (byte)0x05, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0x23, 0xC0, 0xC8, 0x05, 0xCA })]
    public void Ctor_SetsCorrectDataBits(ushort locoAddress, LocoFunctionGroup group, byte functions, byte[] expected)
    {
      SetLocoFunctionGroupCommand command = Factory.Create<SetLocoFunctionGroupCommand>(locoAddress, group, functions);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
