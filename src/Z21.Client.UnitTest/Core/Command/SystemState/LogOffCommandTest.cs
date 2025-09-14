using Z21.Core.Command.SystemState;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class LogOffCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      LogOffCommand command = new();
      Assert.That(
                  command.Data, Is.EqualTo(
                                           new byte[]
                                           {
                                             0x04,
                                             0x00,
                                             0x30,
                                             0x00
                                           }));
    }
  }
}