namespace CommandStation.Model
{
  /// <summary>
  /// Accelerated model railway clock time. <see cref="Day"/> is 0 (Monday) to 6 (Sunday),
  /// <see cref="Rate"/> is the acceleration factor (0–63; 1 = real time).
  /// </summary>
  public record ModelTime(byte Day, byte Hour, byte Minute, byte Second, byte Rate);
}
