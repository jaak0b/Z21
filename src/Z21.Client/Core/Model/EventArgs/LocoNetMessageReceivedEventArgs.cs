namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries a raw LocoNet message (including its checksum) tunneled through the Z21
  /// (<c>LAN_LOCONET_Z21_RX</c>/<c>_TX</c>/<c>_FROM_LAN</c>).
  /// </summary>
  public class LocoNetMessageReceivedEventArgs(byte[] message) : System.EventArgs
  {
    public byte[] Message { get; } = message;
  }
}
