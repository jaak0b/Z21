namespace Z21.Core.Model
{
  /// <summary>
  /// A CAN occupancy detector report (<c>LAN_CAN_DETECTOR</c>, protocol §10.1). The meaning of
  /// <see cref="Value1"/>/<see cref="Value2"/> depends on <see cref="Type"/>.
  /// </summary>
  public record CanDetectorData(ushort NetworkId, ushort ModuleAddress, byte Port, byte Type, ushort Value1, ushort Value2);
}
