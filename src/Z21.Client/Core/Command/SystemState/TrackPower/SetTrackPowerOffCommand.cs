namespace Z21.Core.Command.SystemState.TrackPower
{
  /// <summary>
  /// This command switches off the track voltage.
  /// </summary>
  public class SetTrackPowerOffCommand : IZ21Command
  {
    public string Name => "LAN_X_SET_TRACK_POWER_OFF";

    public byte[] Data { get; } =
      [
        0x07, 0x00,
        0x40, 0x00,
        0x21,
        0x80,
        0x21 ^ 0x80
      ];
  }
}