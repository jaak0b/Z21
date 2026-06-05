namespace Z21.Core.Model
{
  /// <summary>
  /// System state of a 10836 switch decoder (<c>LAN_DECODER_SYSTEMSTATE_DATACHANGED</c>, protocol §11.3.4.1).
  /// Currents are in mA, voltage in mV. <see cref="OutputStates"/>, <see cref="OutputConfig"/> and
  /// <see cref="OutputDimm"/> each have one entry per output (8 outputs).
  /// </summary>
  public record SwitchDecoderSystemState(
    short Current,
    short FilteredCurrent,
    ushort Voltage,
    byte CentralState,
    byte CentralStateEx,
    byte[] OutputStates,
    byte[] OutputConfig,
    byte[] OutputDimm,
    ushort Address,
    ushort Address2,
    byte Dimmed);
}
