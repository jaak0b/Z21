namespace Z21.Core.Model
{
  /// <summary>
  /// The current model time reported by the Z21 (<c>LAN_FAST_CLOCK_DATA</c>, protocol §12.2).
  /// </summary>
  public record FastClockData(byte Day, byte Hour, byte Minute, byte Second, byte Rate, bool IsStopped, bool IsHalted, FastClockSettings Settings);
}
