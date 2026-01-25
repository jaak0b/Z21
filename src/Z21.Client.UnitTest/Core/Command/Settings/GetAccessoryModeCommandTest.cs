using Z21.Core.Command.Settings;

namespace Z21.UnitTest.Core.Command.Settings
{
  public class GetAccessoryModeCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetAccessoryModeCommand command = new(24);
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0x06,
                               0x00,
                               0x70,
                               0x00,
                               0x00,
                               0x18
                             }));
    }
  }
}