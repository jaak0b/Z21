using Z21.Core.Framing;

namespace Z21.Core.Command.FastClock
{
  /// <summary>
  /// Overwrites only the persistent fast-clock setting flags
  /// (<c>LAN_FAST_CLOCK_SETTINGS_SET</c>, protocol §12.4).
  /// </summary>
  public class SetFastClockSettingsCommand : IZ21Command
  {
    public SetFastClockSettingsCommand(IZ21FrameBuilder frameBuilder, byte settings)
    {
      Data = frameBuilder.BuildLan(0x00CF, settings);
    }

    public string Name => "LAN_FAST_CLOCK_SETTINGS_SET";

    public byte[] Data { get; }
  }
}
