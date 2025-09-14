namespace Z21.Core.Command.SystemState.TrackPower
{
  /// <summary>
  /// With this command the emergency stop is activated, i.e. the locomotives are stopped but the track voltage remains switched on.
  /// </summary>
  public class SetStopCommand : IZ21Command
  {
    public string Name => "LAN_X_SET_STOP";

    public byte[] Data { get; } =
      [
        0x06, 0x00,
        0x40, 0x00,
        0x80,
        0x0 ^ 0x80
      ];
  }
}