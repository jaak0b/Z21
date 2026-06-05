namespace Z21.Core.Model
{
  /// <summary>
  /// System state of a 10837 signal decoder (<c>LAN_DECODER_SYSTEMSTATE_DATACHANGED</c>, protocol §11.3.4.2).
  /// Voltage is in mV. <see cref="SignalDccExt"/> carries the current DCCext signal aspect per signal.
  /// </summary>
  public record SignalDecoderSystemState(
    short Current,
    short FilteredCurrent,
    ushort Voltage,
    byte CentralState,
    byte CentralStateEx,
    byte[] OutputStates,
    byte[] BlinkStates,
    byte[] SignalDccExt,
    byte[] SignalCurrentAspect,
    byte SignalCount,
    byte[] SignalConfig,
    byte[] SignalInitAspect,
    ushort Address);
}
