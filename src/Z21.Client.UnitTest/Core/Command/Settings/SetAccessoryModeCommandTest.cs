using Z21.Core.Command.Settings;
using Z21.Core.Model;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Settings
{
  public class SetAccessoryModeCommandTest : CommandTestFixture
  {
    [Test]
    public void Ctor_SetsCorrectDataBits([Values(DecoderMode.DCC, DecoderMode.MM)] DecoderMode decoderMode)
    {
      SetAccessoryModeCommand command = Factory.Create<SetAccessoryModeCommand>((short)12, decoderMode);
      Assert.That(command.Data,
                  Is.EqualTo(new byte[]
                             {
                               0x07,
                               0x00,
                               0x71,
                               0x00,
                               0x00,
                               0x0C,
                               (byte)decoderMode
                             }));
    }

    [Test]
    public void Ctor_LocoModeUnknown_ThrowsArgumentException()
    {
      ArgumentException? exception = Assert.Throws<ArgumentException>(() => _ = Factory.Create<SetAccessoryModeCommand>((short)12, DecoderMode.Unknown));
      Assert.That(exception, Is.Not.Null);
      Assert.That(exception.Message, Is.EqualTo($"{DecoderMode.Unknown} is not a valid DecoderMode. (Parameter 'decoderMode')"));
    }
  }
}
