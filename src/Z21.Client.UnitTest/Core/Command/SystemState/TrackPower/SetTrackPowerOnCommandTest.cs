using Z21.Core.Command.SystemState.TrackPower;

namespace Z21.UnitTest.Core.Command.SystemState.TrackPower
{
  public class SetTrackPowerOnCommandTest
  {
    [Test]
    public void Ctor_SetDataCorrectly()
    {
      SetTrackPowerOnCommand command = new();
      Assert.That(
                  command.Data, Is.EqualTo(
                                           new byte[]
                                           {
                                             0x07, 0x00,
                                             0x40, 0x00,
                                             0x21,
                                             0x81,
                                             0xa0
                                           }));
    }
  }
}