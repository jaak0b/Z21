using Z21.Core.Command.SystemState;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class GetSerialNumberCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetSerialNumberCommand command = Factory.Create<GetSerialNumberCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x04, 0x00, 0x10, 0x00 }));
    }
  }
}
