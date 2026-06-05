using Z21.Core.Command.SystemState.TrackPower;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.SystemState.TrackPower
{
  public class SetTrackPowerOnCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetDataCorrectly()
    {
      SetTrackPowerOnCommand command = Factory.Create<SetTrackPowerOnCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x07, 0x00, 0x40, 0x00, 0x21, 0x81, 0xa0 }));
    }
  }
}
