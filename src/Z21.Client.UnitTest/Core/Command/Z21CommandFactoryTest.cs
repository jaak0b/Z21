using Z21.Core.Command;
using Z21.Core.Command.SystemState;
using Z21.Core.Framing;

namespace Z21.UnitTest.Core.Command
{
  public class Z21CommandFactoryTest : CommandTestFixture
  {
    /// <summary>
    /// A command defined entirely outside the factory: proves a new command needs zero factory edits (open/closed).
    /// </summary>
    private sealed class CustomTestCommand : IZ21Command
    {
      public CustomTestCommand(IZ21FrameBuilder frameBuilder, byte header, byte payload)
      {
        Data = frameBuilder.BuildXBus(header, payload);
      }

      public string Name => "CUSTOM_TEST_COMMAND";

      public byte[] Data { get; }
    }

    [Test]
    public void Create_BuildsCommandDefinedOutsideTheFactory()
    {
      CustomTestCommand command = Factory.Create<CustomTestCommand>((byte)0x21, (byte)0x80);

      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x07, 0x00, 0x40, 0x00, 0x21, 0x80, 0xA1 }));
    }

    [Test]
    public void Create_ResolvesEncodingServicesForParameterlessCommand()
    {
      GetFirmwareVersionCommand command = Factory.Create<GetFirmwareVersionCommand>();

      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x07, 0x00, 0x40, 0x00, 0xF1, 0x0A, 0xFB }));
    }

    [Test]
    public void Ctor_NullFrameBuilder_Throws()
    {
      Assert.Throws<System.ArgumentNullException>(() => new Z21CommandFactory(null!, new Z21.Core.Codecs.AddressCodec(), new Z21.Core.Codecs.LocoSpeedCodec()));
    }

    [Test]
    public void Ctor_NullAddressCodec_Throws()
    {
      Assert.Throws<System.ArgumentNullException>(() => new Z21CommandFactory(new Z21.Core.Framing.Z21FrameBuilder(), null!, new Z21.Core.Codecs.LocoSpeedCodec()));
    }

    [Test]
    public void Ctor_NullLocoSpeedCodec_Throws()
    {
      Assert.Throws<System.ArgumentNullException>(() => new Z21CommandFactory(new Z21.Core.Framing.Z21FrameBuilder(), new Z21.Core.Codecs.AddressCodec(), null!));
    }
  }
}
