namespace CommandStation.Model
{
  /// <summary>
  /// The reported state of an extended accessory decoder.
  /// </summary>
  public record ExtAccessoryInfo(ushort AccessoryAddress, byte EncodedState, bool DataValid);
}
