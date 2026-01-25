using Z21.Core.Command.SystemState;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class GetVersionCommandTest
  {
    [Test]
    public void Ctor_SetsDataCorrectly()
    {
      GetVersionCommand command = new();
      Assert.That(
                  command.Data, Is.EqualTo(
                                           new byte[]
                                           {
                                             0x07, 0x00,
                                             0x40, 0x00,
                                             0x21,
                                             0x21,
                                             0x00
                                           }));
    }
  }
}