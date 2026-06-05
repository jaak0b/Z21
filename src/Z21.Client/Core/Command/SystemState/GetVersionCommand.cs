using Z21.Core.Framing;

namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// The X-Bus version of the Z21 can be read out with the following command.
  /// </summary>
  public class GetVersionCommand : IZ21Command
  {
    public GetVersionCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildXBus(0x21, 0x21);
    }

    public string Name => "LAN_X_GET_VERSION";

    public byte[] Data { get; }
  }
}
