using Z21.Core.Command.SystemState;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class GetSerialNumberCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetSerialNumberCommand getSerialNumberCommand = new();
      Assert.That(
                  getSerialNumberCommand.Data, Is.EqualTo(
                                                             new byte[]
                                                             {
                                                               0x04,
                                                               0x00,
                                                               0x10,
                                                               0x00
                                                             }));
    }
  }
}