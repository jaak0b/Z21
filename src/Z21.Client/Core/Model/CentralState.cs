namespace Z21.Core.Model
{
  /// <summary>
  /// Represents the current operational state of the Z21 central unit.
  /// Each property reflects a specific system condition or mode.
  /// </summary>
  public class CentralState
  {
    /// <summary>
    /// Indicates whether an emergency stop has been triggered.
    /// This halts all train movement immediately.
    /// </summary>
    public bool EmergencyStop { get; init; }

    /// <summary>
    /// Indicates whether the track voltage is currently turned off.
    /// No power is supplied to the rails when this is true.
    /// </summary>
    public bool TrackVoltageOff { get; init; }

    /// <summary>
    /// Indicates whether a short circuit has been detected on the track.
    /// This typically disables power until the fault is resolved.
    /// </summary>
    public bool ShortCircuit { get; init; }

    /// <summary>
    /// Indicates whether the central unit is currently in programming mode.
    /// Used for configuring decoders on the programming track.
    /// </summary>
    public bool ProgrammingModeActive { get; init; }
  }

}