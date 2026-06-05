using System;

namespace CommandStation
{
  /// <summary>
  /// Thrown when a CV programming operation is aborted because the command station reported a short
  /// circuit on the track. The operation is not retried.
  /// </summary>
  public sealed class CvShortCircuitException : Exception
  {
    /// <summary>The 0-based CV address the operation targeted (0 = CV1).</summary>
    public ushort CvAddress { get; }

    public CvShortCircuitException(ushort cvAddress)
      : base($"CV programming aborted: short circuit on the track (CV {cvAddress + 1}).")
    {
      CvAddress = cvAddress;
    }
  }
}
