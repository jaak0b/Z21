using Z21.Core.Framing;

namespace Z21.Core.Command.FastClock
{
  /// <summary>
  /// Overwrites the persistent fast-clock setting flags, the rate and the default start time
  /// (<c>LAN_FAST_CLOCK_SETTINGS_SET</c>, protocol §12.4).
  /// </summary>
  public class SetFastClockSettingsWithStartTimeCommand : IZ21Command
  {
    public SetFastClockSettingsWithStartTimeCommand(IZ21FrameBuilder frameBuilder, byte settings, byte rate, byte startDayHour, byte startMinute)
    {
      Data = frameBuilder.BuildLan(0x00CF, settings, rate, startDayHour, startMinute);
    }

    public string Name => "LAN_FAST_CLOCK_SETTINGS_SET";

    public byte[] Data { get; }
  }
}
