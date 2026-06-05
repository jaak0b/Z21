using Z21.Core.Command.Switching;
using Z21.Core.Model.ExcAccessoryPayload;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Switching
{
  public class SetExtAccessoryCommandTest : CommandTestFixture
  {
    public class ExcAccessoryPayloadDummy(byte payload) : IExcAccessoryPayload
    {
      public byte Payload { get; } = payload;
    }

    [Test]
    public void Ctor_WithPayload_SetsCorrectDataBits()
    {
      SetExtAccessoryCommand command = Factory.Create<SetExtAccessoryCommand>((ushort)1, new ExcAccessoryPayloadDummy(0x52));
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0xA, 0x0,
                               0x40, 0x0,
                               0x54,
                               0x0,
                               0x4,
                               0x52,
                               0x0,
                               0x2
                             }));
    }

    [Test]
    public void Ctor_SetsCorrectDataBits()
    {
      SetExtAccessoryCommand command = Factory.Create<SetExtAccessoryCommand>((ushort)1, (byte)0x05);
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0xA, 0x0,
                               0x40, 0x0,
                               0x54,
                               0x0,
                               0x4,
                               0x5,
                               0x0,
                               0x55
                             }));
    }
  }
}
