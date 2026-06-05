using Z21.Core.Codecs;
using Z21.Core.Command;
using Z21.Core.Framing;

namespace Z21.UnitTest.Core.Command
{
  public abstract class CommandTestFixture
  {
    protected IZ21CommandFactory Factory { get; } =
      new Z21CommandFactory(new Z21FrameBuilder(), new AddressCodec(), new LocoSpeedCodec());
  }
}
