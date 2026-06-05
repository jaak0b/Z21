using Z21.Core.Framing;

namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Reading the broadcast flags in the Z21.
  /// </summary>
  public class GetBroadcastFlagsCommand : IZ21Command
  {
    public GetBroadcastFlagsCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildLan(0x0051);
    }

    public string Name => "LAN_GET_BROADCASTFLAGS";

    public byte[] Data { get; }
  }
}
