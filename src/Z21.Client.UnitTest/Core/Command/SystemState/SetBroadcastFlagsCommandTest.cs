using Z21.Core.Command.SystemState;
using Z21.Core.Model;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class SetBroadcastFlagsCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      SetBroadcastFlagsCommand command = new(
                                             Z21BroadcastFlags.DriveAndSwitchingMessages,
                                             Z21BroadcastFlags.RailComDataChangedMessages);
      Assert.That(
                  command.Data, Is.EqualTo(
                                           new byte[]
                                           {
                                             0x08,
                                             0x0,
                                             0x50,
                                             0x0,
                                             0x5,
                                             0x0,
                                             0x0,
                                             0x0
                                           }));
    }
  }
}