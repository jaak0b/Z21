using System;

namespace CommandStation
{
  /// <summary>
  /// Thrown when a safe CV programming operation does not complete within the caller-supplied timeout
  /// (for example, the decoder never acknowledges so the operation keeps retrying until the deadline).
  /// </summary>
  public sealed class CvOperationTimeoutException : Exception
  {
    /// <summary>The 0-based CV address the operation targeted (0 = CV1).</summary>
    public ushort CvAddress { get; }

    /// <summary>The timeout that elapsed before a result was received.</summary>
    public TimeSpan Timeout { get; }

    public CvOperationTimeoutException(ushort cvAddress, TimeSpan timeout)
      : base($"CV {cvAddress + 1} did not complete within {timeout.TotalSeconds:0.###}s.")
    {
      CvAddress = cvAddress;
      Timeout = timeout;
    }
  }
}
