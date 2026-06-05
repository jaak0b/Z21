using System;
using Z21.Core.Framing;

namespace Z21.Core.Command.Can
{
  /// <summary>
  /// From Z21 FW version 1.41, disables/re-enables the track outputs of a CAN booster
  /// (<c>LAN_CAN_BOOSTER_SET_TRACKPOWER</c>, protocol §10.2.4). 0x00 disables all outputs, 0xFF re-enables.
  /// </summary>
  public class SetCanBoosterTrackPowerCommand : IZ21Command
  {
    public SetCanBoosterTrackPowerCommand(IZ21FrameBuilder frameBuilder, ushort networkId, byte power)
    {
      byte[] nid = BitConverter.GetBytes(networkId);
      Data = frameBuilder.BuildLan(0x00CB, nid[0], nid[1], power);
    }

    public string Name => "LAN_CAN_BOOSTER_SET_TRACKPOWER";

    public byte[] Data { get; }
  }
}
