namespace Z21.Core.Model
{
  /// <summary>
  /// zLink booster system state (<c>LAN_BOOSTER_SYSTEMSTATE_DATACHANGED</c>, protocol §11.2.4). Currents
  /// are in mA, temperatures in °C, voltages in mV. The four central-state bytes are raw bit masks.
  /// </summary>
  public record BoosterSystemState(
    short Booster1MainCurrent,
    short Booster2MainCurrent,
    short Booster1FilteredMainCurrent,
    short Booster2FilteredMainCurrent,
    short Booster1Temperature,
    short Booster2Temperature,
    ushort SupplyVoltage,
    ushort Booster1VccVoltage,
    ushort Booster2VccVoltage,
    byte CentralState,
    byte CentralStateEx,
    byte CentralStateEx2,
    byte CentralStateEx3);
}
