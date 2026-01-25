using Z21.Core.Command.Switching;

namespace Z21.UnitTest.Core.Command.Switching
{
  public class GetExtAccessoryInfoCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetExtAccessoryInfoCommand command = new(15);
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0x9, 0x0,
                               0x40, 0x0,
                               0x44,
                               0x0,
                               0xE,
                               0x0,
                               0x4A
                             }));
    }
  }
}