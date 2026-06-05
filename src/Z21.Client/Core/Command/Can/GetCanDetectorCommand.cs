using System;
using Z21.Core.Framing;

namespace Z21.Core.Command.Can
{
  /// <summary>
  /// From Z21 FW version 1.30, queries a CAN occupancy detector by its CAN network id
  /// (<c>LAN_CAN_DETECTOR</c>, protocol §10.1). Network id 0xD000 queries all CAN detectors.
  /// </summary>
  public class GetCanDetectorCommand : IZ21Command
  {
    public GetCanDetectorCommand(IZ21FrameBuilder frameBuilder, ushort networkId)
    {
      byte[] nid = BitConverter.GetBytes(networkId);
      Data = frameBuilder.BuildLan(0x00C4, 0x00, nid[0], nid[1]);
    }

    public string Name => "LAN_CAN_DETECTOR";

    public byte[] Data { get; }
  }
}
