using Z21.Core.Command.ZLink;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.ZLink
{
  public class GetZLinkHardwareInfoCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_BuildsRequest()
    {
      GetZLinkHardwareInfoCommand command = Factory.Create<GetZLinkHardwareInfoCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x05, 0x00, 0xE8, 0x00, 0x06 }));
    }
  }
}
