using Z21.Core.Command.Driving;

namespace Z21.UnitTest.Core.Command.Driving
{
  public class PurgeLocoCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      PurgeLocoCommand getSerialNumberCommand = new(3);
      Assert.That(getSerialNumberCommand.Data,
                  Is.EqualTo(new byte[]
                             {
                               0x09, 0x00, 0x40, 0x00, 0xE3, 0x44, 0x00, 0x03, 0xA4,
                             }));
    }
  }
}