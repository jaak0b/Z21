using Z21.Core.Command.SystemState;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class GetStatusCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetStatusCommand command = Factory.Create<GetStatusCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x07, 0x00, 0x40, 0x00, 0x21, 0x24, 0x05 }));
    }
  }
}
