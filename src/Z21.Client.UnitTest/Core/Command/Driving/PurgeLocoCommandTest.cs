using Z21.Core.Command.Driving;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Driving
{
  public class PurgeLocoCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      PurgeLocoCommand command = Factory.Create<PurgeLocoCommand>((ushort)3);
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0x09, 0x00, 0x40, 0x00, 0xE3, 0x44, 0x00, 0x03, 0xA4,
                             }));
    }
  }
}
