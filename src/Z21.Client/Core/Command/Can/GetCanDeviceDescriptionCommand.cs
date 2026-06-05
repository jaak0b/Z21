using System;
using Z21.Core.Framing;

namespace Z21.Core.Command.Can
{
  /// <summary>
  /// From Z21 FW version 1.41, reads the free-text description of a CAN booster
  /// (<c>LAN_CAN_DEVICE_GET_DESCRIPTION</c>, protocol §10.2.1).
  /// </summary>
  public class GetCanDeviceDescriptionCommand : IZ21Command
  {
    public GetCanDeviceDescriptionCommand(IZ21FrameBuilder frameBuilder, ushort networkId)
    {
      byte[] nid = BitConverter.GetBytes(networkId);
      Data = frameBuilder.BuildLan(0x00C8, nid[0], nid[1]);
    }

    public string Name => "LAN_CAN_DEVICE_GET_DESCRIPTION";

    public byte[] Data { get; }
  }
}
