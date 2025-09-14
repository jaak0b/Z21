using System;
using System.Net;

// ReSharper disable InconsistentNaming

namespace Z21.Core.Model
{
  public class Z21Configuration
  {
  
    public Z21Configuration(IPEndPoint clientIpEndPoint)
    {
      ArgumentNullException.ThrowIfNull(clientIpEndPoint, nameof(clientIpEndPoint));

      ClientIPEndPoint = clientIpEndPoint;
    }

    /// <summary>
    /// IPEndPoint of the Z21.
    /// </summary>
    public IPEndPoint ClientIPEndPoint { get; }

    /// <summary>
    /// Enables or disables Network Address Translation (NAT) traversal on a UdpClient instance.
    /// </summary>
    public bool AllowNatTraversal { get; set; } = true;

    /// <summary>
    /// Configures the interval in witch the client will send a keep alive command to the z21. This Setting should not need changing!
    /// </summary>
    /// <remarks>The specification states that the client must communicate at least once per minute with the z21 or else the z21 assumes that the client has disconnected.</remarks>
    public TimeSpan ConnectionKeepAliveCommandInterval { get; set; } = TimeSpan.FromSeconds(20);

    /// <summary>
    /// Time it takes between a command being sent and a response being received. This Setting should not need changing!
    /// </summary>
    public TimeSpan ResponseTime { get; set; } = TimeSpan.FromSeconds(2);
    
    /// <summary>
    /// Configures the default broadcast flags that should be sent to the Z21
    /// </summary>
    public uint[] BroadcastFlags { get; set; } =
      [
        Z21BroadcastFlags.DriveAndSwitchingMessages,
        Z21BroadcastFlags.LocoInfoChangedMessages
      ];

    public static class Defaults
    {
      public readonly static IPEndPoint IpEndPoint = new(IPAddress.Parse("192.168.0.111"), 21105);
    }
  }
}