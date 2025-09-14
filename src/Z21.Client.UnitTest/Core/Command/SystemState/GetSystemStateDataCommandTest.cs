using Z21.Core.Command.SystemState;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class GetSystemStateDataCommandTest
  {
    [Test]
    public void Ctor_SetsDataCorrectly()
    {
      GetSystemStateDataCommand command = new();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x04, 0x00, 0x85, 0x00 }));
    }
  }
}