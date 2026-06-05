using Z21.Core.Model;

namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries the hardware information of a Z21 pro LINK (<c>LAN_ZLINK_GET_HWINFO</c> reply).
  /// </summary>
  public class ZLinkHardwareInfoReceivedEventArgs(ZLinkHardwareInfo info) : System.EventArgs
  {
    public ZLinkHardwareInfo Info { get; } = info;
  }
}
