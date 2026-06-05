using Z21.Core.Framing;

namespace Z21.Core.Command.FastClock
{
  /// <summary>
  /// Reads the persistent fast-clock settings (<c>LAN_FAST_CLOCK_SETTINGS_GET</c>, protocol §12.3).
  /// </summary>
  public class GetFastClockSettingsCommand : IZ21Command
  {
    public GetFastClockSettingsCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildLan(0x00CE, 0x04);
    }

    public string Name => "LAN_FAST_CLOCK_SETTINGS_GET";

    public byte[] Data { get; }
  }
}
