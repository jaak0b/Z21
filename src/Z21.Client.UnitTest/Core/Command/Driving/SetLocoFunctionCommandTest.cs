using Z21.Core.Codecs;
using Z21.Core.Command.Driving;
using Z21.Core.Framing;
using Z21.Core.Model;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Driving
{
  public class SetLocoFunctionCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)20, (ushort)0, FunctionToggleType.Off, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0xF8, 0x00, 0x14, 0x00, 0x08 })]
    [TestCase((ushort)125, (ushort)31, FunctionToggleType.On, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0xF8, 0x00, 0x7D, 0x5F, 0x3E })]
    [TestCase((ushort)16, (ushort)5, FunctionToggleType.Toggle, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0xF8, 0x00, 0x10, 0x85, 0x89 })]
    public void Ctor_SetsCorrectDataBits(ushort locoAddress, ushort functionIndex, FunctionToggleType toggleType, byte[] data)
    {
      SetLocoFunctionCommand command = Factory.Create<SetLocoFunctionCommand>(locoAddress, functionIndex, toggleType);
      Assert.That(command.Data, Is.EqualTo(data));
    }

    [Test]
    [TestCase((ushort)64)]
    [TestCase((ushort)255)]
    public void Ctor_FunctionIndexAboveSixBitField_ThrowsArgumentOutOfRange(ushort functionIndex)
    {
      // Spec §4.3.1: DB3 is TTNNNNNN, so the index occupies only the low 6 bits (0..63).
      // A larger value would overflow into the TT toggle-type bits and must be rejected.
      Assert.Throws<System.ArgumentOutOfRangeException>(
        () => new SetLocoFunctionCommand(new Z21FrameBuilder(), new AddressCodec(), 3, functionIndex, FunctionToggleType.Off));
    }
  }
}
