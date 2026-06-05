using Z21.Core.Command.SystemState.TrackPower;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.SystemState.TrackPower
{
  public class SetStopCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetsDataCorrectly()
    {
      SetStopCommand command = Factory.Create<SetStopCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x06, 0x00, 0x40, 0x00, 0x80, 0x80 }));
    }
  }
}
