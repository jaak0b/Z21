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
    /// acknowledge, and returns the value. Requires RailCom; without it (or for an absent decoder) the
    /// read can only ever time out. The result is correlated by CV address only — the protocol's POM
    /// result carries no loco address — so do not run other CV operations on this station concurrently.
    /// </summary>
    /// <exception cref="CvOperationTimeoutException">No result arrived within <paramref name="timeout"/>.</exception>
    /// <exception cref="CvShortCircuitException">The command station reported a short circuit.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout"/> is not a positive, in-range duration.</exception>
    Task<byte> ReadPomCvAsync(ushort locoAddress, ushort cvAddress, TimeSpan timeout);

    /// <summary>
    /// Writes a CV of a locomotive decoder on the main track (POM). Because a POM write returns no
    /// acknowledgement, this verifies by reading the CV back and retrying until the read-back matches
    /// the written value (so it requires RailCom). A decoder that never reads back the written value is
    /// reported as a timeout. Do not run other CV operations on this station concurrently.
    /// </summary>
    /// <exception cref="CvOperationTimeoutException">The write could not be confirmed within <paramref name="timeout"/>.</exception>
    /// <exception cref="CvShortCircuitException">The command station reported a short circuit.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout"/> is not a positive, in-range duration.</exception>
    Task WritePomCvAsync(ushort locoAddress, ushort cvAddress, byte value, TimeSpan timeout);

    /// <summary>
    /// Writes a single bit of a CV of a locomotive decoder on the main track (POM). Because a POM write
    /// returns no acknowledgement, this verifies by reading the CV byte back and retrying until the
    /// target bit matches the written value (so it requires RailCom). A bit that never reads back is
    /// reported as a timeout. Do not run other CV operations on this station concurrently.
    /// </summary>
    /// <param name="bitPosition">The 0-based bit position within the CV (0–7).</param>
    /// <exception cref="CvOperationTimeoutException">The write could not be confirmed within <paramref name="timeout"/>.</exception>
    /// <exception cref="CvShortCircuitException">The command station reported a short circuit.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="bitPosition"/> exceeds 7, or <paramref name="timeout"/> is not a positive, in-range duration.</exception>
    Task WritePomCvBitAsync(ushort locoAddress, ushort cvAddress, byte bitPosition, bool bitValue, TimeSpan timeout);

    /// <summary>
    /// Reads a single bit of a CV of a locomotive decoder on the main track (POM) by reading the CV byte
    /// back (so it requires RailCom) and returning the value of the target bit.
    /// </summary>
    /// <param name="bitPosition">The 0-based bit position within the CV (0–7).</param>
    /// <exception cref="CvOperationTimeoutException">No result arrived within <paramref name="timeout"/>.</exception>
    /// <exception cref="CvShortCircuitException">The command station reported a short circuit.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="bitPosition"/> exceeds 7, or <paramref name="timeout"/> is not a positive, in-range duration.</exception>
    Task<bool> ReadPomCvBitAsync(ushort locoAddress, ushort cvAddress, byte bitPosition, TimeSpan timeout);
  }
}
