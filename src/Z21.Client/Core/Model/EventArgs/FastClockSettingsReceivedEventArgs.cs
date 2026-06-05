using Z21.Core.Model;

namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries the persistent fast-clock settings (<c>LAN_FAST_CLOCK_SETTINGS_GET</c> reply).
  /// </summary>
  public class FastClockSettingsReceivedEventArgs(FastClockSettingsData settings) : System.EventArgs
  {
    public FastClockSettingsData Settings { get; } = settings;
  }
}
