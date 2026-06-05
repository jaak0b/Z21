namespace CommandStation.Model
{
  public enum DecoderMode
  {
    /// <summary>
    /// DCC format
    /// </summary>
    DCC = 0,

    /// <summary>
    /// MM format
    /// </summary>
    MM = 1,

    /// <summary>
    /// Unknown format
    /// </summary>
    Unknown = int.MaxValue
  }
}
