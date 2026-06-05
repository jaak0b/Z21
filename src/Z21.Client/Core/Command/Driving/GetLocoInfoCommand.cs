using Z21.Core.Codecs;
using Z21.Core.Command.SystemState;
using Z21.Core.Framing;
using Z21.Core.Model;

namespace Z21.Core.Command.Driving
{
  /// <summary>
  /// The following command can be used to poll the status of a locomotive. At the same time, the client also "subscribes" to the locomotive information for this locomotive address (only in combination with <see cref="SetBroadcastFlagsCommand"/>, Flag <see cref="Z21BroadcastFlags.DriveAndSwitchingMessages"/>).
  /// </summary>
  public class GetLocoInfoCommand : IZ21Command
  {
    public GetLocoInfoCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort locoAddress)
    {
      (byte lsb, byte msb) = addressCodec.SplitLocoAddress(locoAddress);
      Data = frameBuilder.BuildXBus(0xE3, 0xF0, msb, lsb);
    }

    public string Name => "LAN_X_GET_LOCO_INFO";

    public byte[] Data { get; }
  }
}
