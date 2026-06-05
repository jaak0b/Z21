using Z21.Core.Model;

namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries the current model time (<c>LAN_FAST_CLOCK_DATA</c>).
  /// </summary>
  public class FastClockDataReceivedEventArgs(FastClockData data) : System.EventArgs
  {
    public FastClockData Data { get; } = data;
  }
}
