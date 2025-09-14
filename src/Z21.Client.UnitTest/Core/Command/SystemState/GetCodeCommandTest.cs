using Z21.Core.Command.SystemState;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class GetSoftwareLockCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetSoftwareLockCommand command = new();
      Assert.That(
                  command.Data, Is.EqualTo(
                                           new byte[]
                                           {
                                             0x04,
                                             0x00,
                                             0x18,
                                             0x00
                                           }));
    }
  }
}