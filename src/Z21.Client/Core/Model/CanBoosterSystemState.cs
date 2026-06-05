namespace Z21.Core.Model
{
  /// <summary>
  /// CAN booster system state (<c>LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD</c>, protocol §10.2.3).
  /// </summary>
  public record CanBoosterSystemState(ushort NetworkId, ushort OutputPort, CanBoosterState State, ushort VccVoltage, ushort Current);
}
