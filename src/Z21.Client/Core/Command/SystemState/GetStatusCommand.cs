using Z21.Core.Framing;

namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// This command can be used to request the Z21 status.
  /// </summary>
  public class GetStatusCommand : IZ21Command
  {
    public GetStatusCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildXBus(0x21, 0x24);
    }

    public string Name => "LAN_X_GET_STATUS";

    public byte[] Data { get; }
  }
}
