using Z21.Core.Command.SystemState;
using Z21.Core.Model;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class SetBroadcastFlagsCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      SetBroadcastFlagsCommand command = Factory.Create<SetBroadcastFlagsCommand>(
                                                                                  new[]
                                                                                  {
                                                                                    Z21BroadcastFlags.DriveAndSwitchingMessages,
                                                                                    Z21BroadcastFlags.RailComDataChangedMessages
                                                                                  });
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x08, 0x0, 0x50, 0x0, 0x5, 0x0, 0x0, 0x0 }));
    }

    [Test]
    public void Ctor_NoFlags_EncodesZero()
    {
      SetBroadcastFlagsCommand command = Factory.Create<SetBroadcastFlagsCommand>(new uint[0]);
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x08, 0x00, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00 }));
    }

    [Test]
    public void Name_IsLanSetBroadcastFlags()
    {
      SetBroadcastFlagsCommand command = Factory.Create<SetBroadcastFlagsCommand>(new[] { Z21BroadcastFlags.DriveAndSwitchingMessages });
      Assert.That(command.Name, Is.EqualTo("LAN_SET_BROADCASTFLAGS"));
    }
  }
}
