namespace CommandStation.Model
{
  /// <summary>
  /// Represents extended diagnostic states reported by a command station.
  /// These flags provide additional system-level status information beyond the basic central state.
  /// </summary>
  public class CentralStateEx
  {
    /// <summary>
    /// Indicates that the central unit has reached a high internal temperature.
    /// This may trigger automatic shutdown or reduced performance to prevent damage.
    /// </summary>
    public bool HighTemperature { get; init; }

    /// <summary>
    /// Indicates that the central unit has lost its power supply.
    /// This typically means the device is running on backup or has shut down.
    /// </summary>
    public bool PowerLost { get; init; }

    /// <summary>
    /// Indicates that an external short circuit has been detected.
    /// This usually refers to a fault in connected boosters or track wiring.
    /// </summary>
    public bool ShortCircuitExternal { get; init; }

    /// <summary>
    /// Indicates that an internal short circuit has been detected within the central unit.
    /// This may require hardware inspection or service.
    /// </summary>
    public bool ShortCircuitInternal { get; init; }

    /// <summary>
    /// Indicates that RCN-213 protocol-specific conditions are active.
    /// This flag is used for RailCom diagnostics and may relate to feedback or communication issues.
    /// </summary>
    public bool Rcn213 { get; init; }
  }
}
