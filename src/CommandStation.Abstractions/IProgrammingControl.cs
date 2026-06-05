using System;
using System.Threading.Tasks;
using CommandStation.Model;

namespace CommandStation
{
  /// <summary>
  /// Reading and writing decoder configuration variables (CVs) in direct mode on the programming track.
  /// CV addresses are 0-based (0 = CV1).
  /// </summary>
  public interface IProgrammingControl
  {
    Task ReadCvAsync(ushort cvAddress);

    Task WriteCvAsync(ushort cvAddress, byte value);

    /// <summary>
    /// Reads a CV on the programming track, retrying while the decoder does not acknowledge, and
    /// returns the value. A missing acknowledgement that never clears (for example an absent or
    /// unreadable decoder) is reported as a timeout rather than a distinct error. Do not call the
    /// fire-and-forget CV methods concurrently with this one on the same station.
    /// </summary>
    /// <exception cref="CvOperationTimeoutException">No result arrived within <paramref name="timeout"/>.</exception>
    /// <exception cref="CvShortCircuitException">The command station reported a short circuit.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout"/> is not a positive, in-range duration.</exception>
    Task<byte> ReadCvAsync(ushort cvAddress, TimeSpan timeout);

    /// <summary>
    /// Writes a CV on the programming track, retrying while the decoder does not acknowledge, and
    /// completes once the command station acknowledges the write (<c>LAN_X_CV_RESULT</c>). A missing
    /// acknowledgement that never clears is reported as a timeout. Do not call the fire-and-forget CV
    /// methods concurrently with this one on the same station.
    /// </summary>
    /// <exception cref="CvOperationTimeoutException">The write was not acknowledged within <paramref name="timeout"/>.</exception>
    /// <exception cref="CvShortCircuitException">The command station reported a short circuit.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout"/> is not a positive, in-range duration.</exception>
    Task WriteCvAsync(ushort cvAddress, byte value, TimeSpan timeout);

    event EventHandler<CvValue>? CvReadCompleted;

    event EventHandler<CvProgrammingError>? CvProgrammingFailed;
  }
}
