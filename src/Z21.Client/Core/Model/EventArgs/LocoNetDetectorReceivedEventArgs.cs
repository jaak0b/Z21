namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries a LocoNet occupancy detector report (<c>LAN_LOCONET_DETECTOR</c>). The meaning of
  /// <see cref="Info"/> depends on <see cref="Type"/> (see protocol §9.5).
  /// </summary>
  public class LocoNetDetectorReceivedEventArgs(byte type, ushort reportAddress, byte[] info) : System.EventArgs
  {
    public byte Type { get; } = type;

    public ushort ReportAddress { get; } = reportAddress;

    public byte[] Info { get; } = info;
  }
}
