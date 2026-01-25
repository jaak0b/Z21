using Z21.Core.Command.SystemState;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class GetBroadcastFlagsCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetBroadcastFlagsCommand getSerialNumberCommand = new();
      Assert.That(
                  getSerialNumberCommand.Data, Is.EqualTo(
                                                          new byte[]
                                                          {
                                                            0x04,
                                                            0x00,
                                                            0x51,
                                                            0x00
                                                          }));
    }
  }
}