using System;
using System.Threading.Tasks;

namespace CommandStation
{
  /// <summary>
  /// Track power and global emergency stop.
  /// </summary>
  public interface ITrackPowerControl
  {
    Task TrackPowerOnAsync();

    Task TrackPowerOffAsync();

    /// <summary>
    /// Stops all locomotives while leaving the track voltage on.
    /// </summary>
    Task EmergencyStopAllAsync();

    /// <summary>
    /// Raised when track power is switched on (true) or off (false).
    /// </summary>
    event EventHandler<bool>? TrackPowerChanged;
  }
}
