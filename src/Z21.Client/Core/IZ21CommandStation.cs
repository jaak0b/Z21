using System;
using System.Threading.Tasks;
using CommandStation;
using Z21.Core.Command;

namespace Z21.Core
{
  /// <summary>
  /// The Z21 command station: the protocol-agnostic capabilities plus a Z21-specific raw escape hatch
  /// for sending hand-built commands.
  /// </summary>
  public interface IZ21CommandStation : ICommandStation, ILocoControl, IAccessoryControl, ITrackPowerControl, ISystemInfoProvider, IProgrammingControl, IFeedbackControl, IFastClockControl
  {
    /// <summary>
    /// Factory for building raw Z21 commands to pass to <see cref="SendCommandsAsync"/>.
    /// </summary>
    IZ21CommandFactory Commands { get; }

    /// <summary>
    /// Sends one or more raw commands in a single UDP packet.
    /// </summary>
    Task SendCommandsAsync(params IZ21Command[] commands);

    /// <summary>
    /// Reads a CV of a locomotive decoder on the main track (POM), retrying while the decoder does not
    /// acknowledge, and returns the value. Requires RailCom; without it the read times out. Throws
    /// <see cref="CvOperationTimeoutException"/> on timeout or <see cref="CvShortCircuitException"/> on
    /// a short circuit.
    /// </summary>
    Task<byte> ReadPomCvAsync(ushort locoAddress, ushort cvAddress, TimeSpan timeout);

    /// <summary>
    /// Writes a CV of a locomotive decoder on the main track (POM). Because a POM write returns no
    /// acknowledgement, this verifies by reading the CV back and retrying until the read-back matches
    /// the written value (so it requires RailCom). Throws <see cref="CvOperationTimeoutException"/> if
    /// it cannot be confirmed within <paramref name="timeout"/>.
    /// </summary>
    Task WritePomCvAsync(ushort locoAddress, ushort cvAddress, byte value, TimeSpan timeout);
  }
}
