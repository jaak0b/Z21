namespace CommandStation.Model
{
  /// <summary>
  /// Represents the supported capabilities of a command station device.
  /// Each property corresponds to a specific protocol feature.
  /// </summary>
  public class Capabilities
  {
    /// <summary>
    /// Indicates whether the device supports DCC (Digital Command Control) protocol.
    /// </summary>
    public bool Dcc { get; init; }

    /// <summary>
    /// Indicates whether the device supports Märklin Motorola (MM) protocol.
    /// </summary>
    public bool Mm { get; init; }

    /// <summary>
    /// Indicates whether the device supports RailCom feedback functionality.
    /// </summary>
    public bool RailCom { get; init; }

    /// <summary>
    /// Indicates whether the device can send locomotive commands.
    /// </summary>
    public bool LocoCmds { get; init; }

    /// <summary>
    /// Indicates whether the device can send accessory commands (e.g., turnouts, signals).
    /// </summary>
    public bool AccessoryCmds { get; init; }

    /// <summary>
    /// Indicates whether the device can send detector commands (e.g., feedback modules).
    /// </summary>
    public bool DetectorCmds { get; init; }

    /// <summary>
    /// Indicates whether the device requires an unlock code to access certain features.
    /// </summary>
    public bool NeedsUnlockCode { get; init; }
  }
}
