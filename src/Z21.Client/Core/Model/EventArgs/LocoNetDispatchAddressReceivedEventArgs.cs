namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries the result of a LocoNet dispatch request (<c>LAN_LOCONET_DISPATCH_ADDR</c>).
  /// <see cref="Slot"/> 0 indicates the dispatch failed; a positive value is the assigned LocoNet slot.
  /// </summary>
  public class LocoNetDispatchAddressReceivedEventArgs(ushort locoAddress, byte slot) : System.EventArgs
  {
    public ushort LocoAddress { get; } = locoAddress;

    public byte Slot { get; } = slot;
  }
}
