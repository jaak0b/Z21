using Z21.Core.Command.Driving;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Driving
{
  public class SetLocoEStopCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      SetLocoEStopCommand command = Factory.Create<SetLocoEStopCommand>((ushort)3);
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0x08, 0x00, 0x40, 0x00, 0x92, 0x00, 0x03, 0x91
                             }));
    }
  }
}
