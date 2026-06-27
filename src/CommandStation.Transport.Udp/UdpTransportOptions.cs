using System.Net;

namespace CommandStation.Transport.Udp
{
  public class UdpTransportOptions
  {
    /// <summary>
    /// The remote endpoint of the command station.
    /// </summary>
    public IPEndPoint RemoteEndPoint { get; set; } = new(IPAddress.Parse(DefaultAddress), DefaultPort);

    /// <summary>
    /// Enables or disables NAT traversal on the underlying socket (Windows only).
    /// </summary>
    public bool AllowNatTraversal { get; set; } = true;

    /// <summary>
    /// Local UDP port to bind. <c>null</c> binds the command station's port, because some Z21 firmware
    /// sends LAN_X_LOCO_INFO broadcasts there rather than to the client's source port; <c>0</c> lets the
    /// OS assign an ephemeral port; any other value binds that port.
    /// </summary>
    public int? LocalPort { get; set; }

    public const string DefaultAddress = "192.168.0.111";

    public const int DefaultPort = 21105;
  }
}
