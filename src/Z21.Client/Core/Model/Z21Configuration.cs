using System;
using System.Net;

// ReSharper disable InconsistentNaming

namespace Z21.Core.Model
{
  public class Z21Configuration
  {
    private IPEndPoint _clientIpEndPoint = Defaults.IpEndPoint;
    private bool _allowNatTraversal = true;

    /// <summary>
    /// IPEndPoint of the Z21.
    /// </summary>
    public IPEndPoint ClientIPEndPoint
    {
      get => _clientIpEndPoint;
      set
      {
        ArgumentNullException.ThrowIfNull(value);
        if (_clientIpEndPoint.Equals(value))
          return;
        
        _clientIpEndPoint = value;
        ConfigurationUpdated?.Invoke(this, System.EventArgs.Empty);
      } 
    }

    /// <summary>
    /// Enables or disables Network Address Translation (NAT) traversal on a UdpClient instance.
    /// </summary>
    public bool AllowNatTraversal
    {
      get => _allowNatTraversal;
      set
      {
        if (_allowNatTraversal.Equals(value))
          return;
        _allowNatTraversal = value;
        ConfigurationUpdated?.Invoke(this, System.EventArgs.Empty);
      }
    }

    /// <summary>
    /// Time it takes between a command being sent and a response being received. This Setting should not need changing!
    /// </summary>
    public TimeSpan ResponseTime { get; set; } = TimeSpan.FromSeconds(2);

    /// <summary>
    /// Configures the default broadcast flags that should be sent to the Z21
    /// </summary>
    public uint[] BroadcastFlags { get; set; } = Defaults.BroadcastFlags;

    public event EventHandler<System.EventArgs>? ConfigurationUpdated;
      
    public static class Defaults
    {
      public readonly static IPEndPoint IpEndPoint = new(IPAddress.Parse("192.168.0.111"), 21105);
      
      public readonly static uint[] BroadcastFlags =
      [
        Z21BroadcastFlags.DriveAndSwitchingMessages,
        Z21BroadcastFlags.LocoInfoChangedMessages
      ];
    }
  }
}