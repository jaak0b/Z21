namespace Z21.Core.Model
{
  /// <summary>
  /// The persistent fast-clock settings (<c>LAN_FAST_CLOCK_SETTINGS_GET</c> reply, protocol §12.3).
  /// </summary>
  public record FastClockSettingsData(FastClockSettings Settings, byte Rate, byte StartDayHour, byte StartMinute);
}
