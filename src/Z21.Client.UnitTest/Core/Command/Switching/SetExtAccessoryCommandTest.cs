using Z21.Core.Command.Switching;
using Z21.Core.Model.ExcAccessoryPayload;

namespace Z21.UnitTest.Core.Command.Switching
{
  public class SetExtAccessoryCommandTest
  {
    public class ExcAccessoryPayloadDummy(byte payload) : IExcAccessoryPayload
    {
      public byte Payload { get; } = payload;
    }

    [Test]
    public void Ctor_WithPayload_SetsCorrectDataBits()
    {
      SetExtAccessoryCommand command = new(15, new ExcAccessoryPayloadDummy(0x52));
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0xA, 0x0,
                               0x40, 0x0,
                               0x54,
                               0x0,
                               0xE,
                               0x52,
                               0x0,
                               0x8
                             }));
    }

    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      SetExtAccessoryCommand command = new(15, 0x62);
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0xA, 0x0,
                               0x40, 0x0,
                               0x54,
                               0x0,
                               0xE,
                               0x62,
                               0x0,
                               0x38
                             }));
    }
  }
}