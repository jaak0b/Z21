using Z21.Core.Framing;

namespace Z21.Core.Command.SystemState.TrackPower
{
  /// <summary>
  /// This command switches off the track voltage.
  /// </summary>
  public class SetTrackPowerOffCommand : IZ21Command
  {
    public SetTrackPowerOffCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildXBus(0x21, 0x80);
    }

    public string Name => "LAN_X_SET_TRACK_POWER_OFF";

    public byte[] Data { get; }
  }
}
