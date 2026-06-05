using Z21.Core.Framing;

namespace Z21.Core.Command.SystemState.TrackPower
{
  /// <summary>
  /// With this command the emergency stop is activated, i.e. the locomotives are stopped but the track voltage remains switched on.
  /// </summary>
  public class SetStopCommand : IZ21Command
  {
    public SetStopCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildXBus(0x80);
    }

    public string Name => "LAN_X_SET_STOP";

    public byte[] Data { get; }
  }
}
