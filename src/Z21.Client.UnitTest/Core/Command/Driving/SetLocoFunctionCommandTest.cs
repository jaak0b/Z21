using Z21.Core.Command.Driving;
using Z21.Core.Model;

namespace Z21.UnitTest.Core.Command.Driving
{
  public class SetLocoFunctionCommandTest
  {
    [Test]
    [TestCase((ushort)20, (ushort)0, FunctionToggleType.Off, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0xF8, 0x00, 0x14, 0x00, 0x08 })]
    [TestCase((ushort)125, (ushort)31, FunctionToggleType.On, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0xF8, 0x00, 0x7D, 0x5F, 0x3E })]
    [TestCase((ushort)16, (ushort)5, FunctionToggleType.Toggle, new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0xF8, 0x00, 0x10, 0x85, 0x89 })]
    public void Ctor_SetsCorrectDataBits(ushort locoAddress, ushort functionIndex, FunctionToggleType toggleType, byte[] data)
    {
      SetLocoFunctionCommand command = new(locoAddress, functionIndex, toggleType);
      Assert.That(command.Data, Is.EqualTo(data));
    }
  }
}