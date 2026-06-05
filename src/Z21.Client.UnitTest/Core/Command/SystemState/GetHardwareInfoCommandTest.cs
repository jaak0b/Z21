using Z21.Core.Command.SystemState;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class GetHardwareInfoCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetHardwareInfoCommand command = Factory.Create<GetHardwareInfoCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x04, 0x00, 0x1A, 0x00 }));
    }
  }
}
