using Z21.Core.Command.Switching;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Switching
{
  public class GetExtAccessoryInfoCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetExtAccessoryInfoCommand command = Factory.Create<GetExtAccessoryInfoCommand>((ushort)1);
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0x9, 0x0,
                               0x40, 0x0,
                               0x44,
                               0x0,
                               0x4,
                               0x0,
                               0x40
                             }));
    }
  }
}
