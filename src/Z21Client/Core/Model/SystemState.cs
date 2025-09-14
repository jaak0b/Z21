namespace Z21.Core.Model
{
  /// <summary>
  /// Represents the complete system status of the Z21 central unit,
  /// including electrical measurements, temperature, and operational flags.
  /// </summary>
  public class SystemState
  {
    /// <summary>
    /// The current (in milliamps) drawn by the main track output.
    /// </summary>
    public int MainCurrent { get; init; }

    /// <summary>
    /// The current (in milliamps) drawn by the programming track output.
    /// </summary>
    public int ProgCurrent { get; init; }

    /// <summary>
    /// The filtered current (in milliamps) of the main track output.
    /// This value may be smoothed or averaged for diagnostics.
    /// </summary>
    public int FilteredMainCurrent { get; init; }

    /// <summary>
    /// The internal temperature of the Z21 unit (in degrees Celsius).
    /// </summary>
    public int Temperature { get; init; }

    /// <summary>
    /// The supply voltage (in millivolts) provided to the Z21 unit.
    /// </summary>
    public int SupplyVoltage { get; init; }

    /// <summary>
    /// The internal Vcc voltage (in millivolts) used by the Z21 logic circuits.
    /// </summary>
    public int VccVoltage { get; init; }

    /// <summary>
    /// The basic operational state of the Z21 central unit,
    /// including emergency stop, voltage status, and programming mode.
    /// </summary>
    public required CentralState CentralState { get; init; }

    /// <summary>
    /// Extended diagnostic flags from the Z21 central unit,
    /// such as temperature warnings, power loss, and short circuit conditions.
    /// </summary>
    public required CentralStateEx CentralStateEx { get; init; }

    // Reserved field for future use or protocol alignment.
    // public int? Reserved { get; init; }

    /// <summary>
    /// The set of capabilities supported by the Z21 device,
    /// such as RailCom, LocoNet, and accessory command support.
    /// Will be null on older firmware versions.
    /// </summary>
    public Capabilities? Capabilities { get; init; }
  }
}