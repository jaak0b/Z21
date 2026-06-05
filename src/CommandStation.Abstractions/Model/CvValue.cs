namespace CommandStation.Model
{
  /// <summary>
  /// The value of a decoder configuration variable read back from the command station.
  /// <see cref="CvAddress"/> is 0-based (0 = CV1).
  /// </summary>
  public record CvValue(ushort CvAddress, byte Value);
}
