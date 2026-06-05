namespace CommandStation.Model
{
  /// <summary>
  /// The reported state of a turnout/accessory. <see cref="Output"/> is null when the turnout has
  /// not yet been switched or was switched with an invalid combination.
  /// </summary>
  public record TurnoutInfo(ushort AccessoryAddress, AccessoryOutput? Output);
}
