using Z21.Core.Command.SystemState.TrackPower;

namespace Z21.UnitTest.Core.Command.SystemState.TrackPower
{
  public class SetTrackPowerOffCommandTest
  {
    [Test]
    public void Ctor_SetsDataCorrectly()
    {
      SetTrackPowerOffCommand command = new();
      Assert.That(
                  command.Data, Is.EqualTo(
                                           new byte[]
                                           {
                                             0x07, 0x00,
                                             0x40, 0x00,
                                             0x21,
                                             0x80,
                                             0xa1
                                           }));
    }
  }
}