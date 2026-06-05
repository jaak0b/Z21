using Z21.Core.Framing;

namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Logging off the client from the Z21.
  /// </summary>
  public class LogOffCommand : IZ21Command
  {
    public LogOffCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildLan(0x0030);
    }

    public string Name => "LAN_LOGOFF";

    public byte[] Data { get; }
  }
}
