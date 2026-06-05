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

    event EventHandler<CvValue>? CvReadCompleted;

    event EventHandler<CvProgrammingError>? CvProgrammingFailed;
  }
}
