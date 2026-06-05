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

    public const string DefaultAddress = "192.168.0.111";

    public const int DefaultPort = 21105;
  }
}
