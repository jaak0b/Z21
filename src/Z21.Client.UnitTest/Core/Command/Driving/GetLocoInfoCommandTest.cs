using Z21.Core.Command.Driving;

namespace Z21.UnitTest.Core.Command.Driving
{
  public class GetLocoInfoCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetLocoInfoCommand command = new(3);
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0x09, 0x00,
                               0x40, 0x00,
                               0xE3,
                               0xF0,
                               0x00,
                               0x03,
                               0x10
                             }));
    }
  }
}