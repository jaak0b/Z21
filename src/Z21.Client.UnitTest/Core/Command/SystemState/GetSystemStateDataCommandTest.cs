using Z21.Core.Command.SystemState;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class GetSystemStateDataCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetsDataCorrectly()
    {
      GetSystemStateDataCommand command = Factory.Create<GetSystemStateDataCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x04, 0x00, 0x85, 0x00 }));
    }
  }
}
