using Z21.Core.Command.Switching;

namespace Z21.UnitTest.Core.Command.Switching
{
  public class GetTurnoutInfoCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetTurnoutInfoCommand command = new(15);
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0x08, 0x00, 0x40, 0x00, 0x43, 0x00, 0x0E, 0x4D
                             }));
    }

    [Test]
    public void Ctor_AccessoryAddressIs0_ThrowsArgumentOutOfRangeException()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => _ = new GetTurnoutInfoCommand(0));
    }
  }
}