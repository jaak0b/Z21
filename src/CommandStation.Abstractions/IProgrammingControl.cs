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
    /// returns the value. Throws <see cref="CvOperationTimeoutException"/> if no result arrives within
    /// <paramref name="timeout"/>, or <see cref="CvShortCircuitException"/> on a short circuit.
    /// </summary>
    Task<byte> ReadCvAsync(ushort cvAddress, TimeSpan timeout);

    /// <summary>
    /// Writes a CV on the programming track, retrying while the decoder does not acknowledge, and
    /// completes once the command station confirms the write. Throws
    /// <see cref="CvOperationTimeoutException"/> if not confirmed within <paramref name="timeout"/>, or
    /// <see cref="CvShortCircuitException"/> on a short circuit.
    /// </summary>
    Task WriteCvAsync(ushort cvAddress, byte value, TimeSpan timeout);

    event EventHandler<CvValue>? CvReadCompleted;

    event EventHandler<CvProgrammingError>? CvProgrammingFailed;
  }
}
