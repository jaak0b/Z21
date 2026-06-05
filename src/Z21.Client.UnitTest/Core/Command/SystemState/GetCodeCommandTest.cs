using Z21.Core.Command.SystemState;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class GetSoftwareLockCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetSoftwareLockCommand command = Factory.Create<GetSoftwareLockCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x04, 0x00, 0x18, 0x00 }));
    }
  }
}
