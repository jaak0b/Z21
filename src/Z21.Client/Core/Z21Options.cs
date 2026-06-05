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

    /// <summary>
    /// Delay between retries of a safe (timeout-bounded) CV operation after the decoder fails to
    /// acknowledge (<c>LAN_X_CV_NACK</c>). A short, non-zero delay avoids hammering the command station
    /// and repeatedly re-entering programming mode while a slow byte-wise read is in progress. The
    /// caller-supplied timeout still bounds the overall operation.
    /// </summary>
    public TimeSpan CvRetryDelay { get; set; } = TimeSpan.FromMilliseconds(50);
  }
}
