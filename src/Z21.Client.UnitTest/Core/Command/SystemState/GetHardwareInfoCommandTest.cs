using Z21.Core.Command.SystemState;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class GetHardwareInfoCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetHardwareInfoCommand command = new();
      Assert.That(
                  command.Data, Is.EqualTo(
                                           new byte[]
                                           {
                                             0x04,
                                             0x00,
                                             0x1A,
                                             0x00
                                           }));
    }
  }
}