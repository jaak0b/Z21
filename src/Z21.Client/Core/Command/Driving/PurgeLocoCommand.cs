using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Driving
{
  /// <summary>
  /// From Z21 FW version 1.43, a locomotive can be removed from the Z21 with the following command.
  /// This also cancels the sending of the loco commands for this locomotive on the track.
  /// Sending will start again as soon as a new drive or function command is sent to the same locomotive address.
  /// </summary>
  public class PurgeLocoCommand : IZ21Command
  {
    public PurgeLocoCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort locoAddress)
    {
      (byte lsb, byte msb) = addressCodec.SplitLocoAddress(locoAddress);
      Data = frameBuilder.BuildXBus(0xE3, 0x44, msb, lsb);
    }

    public string Name => "LAN_X_PURGE_LOCO";

    public byte[] Data { get; }
  }
}
