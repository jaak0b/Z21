using Z21.Core.Command.Switching;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Switching
{
  public class GetTurnoutInfoCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetTurnoutInfoCommand command = Factory.Create<GetTurnoutInfoCommand>((ushort)15);
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0x08, 0x00, 0x40, 0x00, 0x43, 0x00, 0x0E, 0x4D
                             }));
    }

    [Test]
    public void Ctor_AccessoryAddressIs0_ThrowsArgumentOutOfRangeException()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => _ = Factory.Create<GetTurnoutInfoCommand>((ushort)0));
    }
  }
}
