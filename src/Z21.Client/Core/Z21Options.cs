using System;
using Z21.Core.Model;

namespace Z21.Core
{
  /// <summary>
  /// Protocol-level options for the Z21 command station (transport options are configured separately
  /// on the transport itself).
  /// </summary>
  public class Z21Options
  {
    /// <summary>
    /// Broadcast flags requested from the Z21 on (re)connect.
    /// </summary>
    public uint[] BroadcastFlags { get; set; } =
    [
      Z21BroadcastFlags.DriveAndSwitchingMessages,
      Z21BroadcastFlags.LocoInfoChangedMessages
    ];

    /// <summary>
    /// Interval after the last command before an automatic keep-alive request is sent.
    /// </summary>
    public TimeSpan KeepAliveInterval { get; set; } = TimeSpan.FromSeconds(45);
  }
}
