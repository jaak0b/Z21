namespace Z21.Core.Model
{
  /// <summary>
  /// RailCom data reported by the Z21 for a decoder (<c>LAN_RAILCOM_DATACHANGED</c>, protocol §8.1).
  /// </summary>
  public record RailComData(ushort LocoAddress, uint ReceiveCounter, ushort ErrorCounter, RailComOptions Options, byte Speed, byte QualityOfService);
}
