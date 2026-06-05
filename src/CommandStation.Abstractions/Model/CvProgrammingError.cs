namespace CommandStation.Model
{
  /// <summary>
  /// Why a CV programming operation failed.
  /// </summary>
  public enum CvProgrammingError
  {
    /// <summary>No decoder acknowledgement was received.</summary>
    NoAcknowledgement,

    /// <summary>Programming failed because of a short circuit on the track.</summary>
    ShortCircuit
  }
}
