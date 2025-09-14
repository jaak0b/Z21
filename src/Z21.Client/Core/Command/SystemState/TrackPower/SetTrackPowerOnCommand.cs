namespace Z21.Core.Command.SystemState.TrackPower
{
  /// <summary>
  /// This command switches on the track voltage, or terminates either the emergency stop or the programming mode.
  /// </summary>
  public class SetTrackPowerOnCommand : IZ21Command
  {
    public string Name => "LAN_X_SET_TRACK_POWER_ON";

    public byte[] Data { get; } =
      [
        0x07, 0x00,
        0x40, 0x00,
        0x21,
        0x81,
        0x21 ^ 0x81
      ];
  }
}