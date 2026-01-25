using Z21.Core.Command.SystemState;

namespace Z21.UnitTest.Core.Command.SystemState
{
  public class GetFirmwareVersionCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      GetFirmwareVersionCommand getSerialNumberCommand = new();
      Assert.That(
                  getSerialNumberCommand.Data, Is.EqualTo(
                                                          new byte[]
                                                          {
                                                            0x07,
                                                            0x00,
                                                            0x40,
                                                            0x00,
                                                            0xF1,
                                                            0x0A,
                                                            0xFB
                                                          }));
    }
  }
}