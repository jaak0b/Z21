using Z21.Core.Command.Switching;
using Z21.Core.Model;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Switching
{
  public class SetTurnoutCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)16, AccessoryOutput.Output1, AccessoryState.Activate, true, new byte[] { 0x09, 0x00, 0x40, 0x00, 0x53, 0x00, 0x0F, 0x88, 0xD4 })]
    [TestCase((ushort)8, AccessoryOutput.Output2, AccessoryState.Activate, true, new byte[] { 0x09, 0x00, 0x40, 0x00, 0x53, 0x00, 0x07, 0x89, 0xDD })]
    [TestCase((ushort)8, AccessoryOutput.Output2, AccessoryState.Deactivate, true, new byte[] { 0x09, 0x00, 0x40, 0x00, 0x53, 0x00, 0x07, 0x81, 0xD5 })]
    [TestCase((ushort)8, AccessoryOutput.Output2, AccessoryState.Deactivate, false, new byte[] { 0x09, 0x00, 0x40, 0x00, 0x53, 0x00, 0x07, 0xA1, 0xF5 })]
    public void Ctor_SetsCorrectDataBits(ushort accessoryAddress, AccessoryOutput accessoryOutput, AccessoryState accessoryState, bool executeImmediately, byte[] data)
    {
      SetTurnoutCommand command = Factory.Create<SetTurnoutCommand>(accessoryAddress, accessoryOutput, accessoryState, executeImmediately);
      Assert.That(command.Data, Is.EqualTo(data));
    }

    [Test]
    public void Ctor_AccessoryAddressIs0_ThrowsArgumentOutOfRangeException()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => _ = Factory.Create<SetTurnoutCommand>((ushort)0, AccessoryOutput.Output1, AccessoryState.Activate, false));
    }
  }
}
