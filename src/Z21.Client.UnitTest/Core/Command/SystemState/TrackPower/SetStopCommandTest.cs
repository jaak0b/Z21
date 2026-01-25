using Z21.Core.Command.SystemState.TrackPower;

namespace Z21.UnitTest.Core.Command.SystemState.TrackPower
{
  public class SetStopCommandTest
  {
    [Test]
    public void Ctor_SetsDataCorrectly()
    {
      SetStopCommand command = new();
      Assert.That(
                  command.Data, Is.EqualTo(
                                           new byte[]
                                           {
                                             0x06, 0x00,
                                             0x40, 0x00,
                                             0x80,
                                             (0 ^ 0x80)
                                           }));
    }
  }
}