using Z21.Core.Framing;

namespace Z21.Core.Command.FastClock
{
  /// <summary>
  /// Overwrites the persistent fast-clock setting flags and the rate
  /// (<c>LAN_FAST_CLOCK_SETTINGS_SET</c>, protocol §12.4).
  /// </summary>
  public class SetFastClockSettingsWithRateCommand : IZ21Command
  {
    public SetFastClockSettingsWithRateCommand(IZ21FrameBuilder frameBuilder, byte settings, byte rate)
    {
      Data = frameBuilder.BuildLan(0x00CF, settings, rate);
    }

    public string Name => "LAN_FAST_CLOCK_SETTINGS_SET";

    public byte[] Data { get; }
  }
}
