using Z21.Core.Command.Driving;

namespace Z21.UnitTest.Core.Command.Driving
{
  public class SetLocoEStopCommandTest
  {
    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      SetLocoEStopCommand getSerialNumberCommand = new(3);
      Assert.That(getSerialNumberCommand.Data,
                  Is.EqualTo(new byte[]
                             {
                               0x08, 0x00, 0x40, 0x00, 0x92, 0x00, 0x03, 0x91
                             }));
    }
  }
}