using Z21.Core.Framing;

namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Request the current system status.
  /// </summary>
  public class GetSystemStateDataCommand : IZ21Command
  {
    public GetSystemStateDataCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildLan(0x0085);
    }

    public string Name => "LAN_SYSTEMSTATE_GETDATA";

    public byte[] Data { get; }
  }
}
