using System;
using Z21.Core.Framing;

namespace Z21.Core.Command.LocoNet
{
  /// <summary>
  /// From Z21 FW version 1.22, queries the occupancy status of LocoNet track occupancy detectors
  /// (<c>LAN_LOCONET_DETECTOR</c>, protocol §9.5).
  /// </summary>
  public class LocoNetDetectorCommand : IZ21Command
  {
    public LocoNetDetectorCommand(IZ21FrameBuilder frameBuilder, byte type, ushort reportAddress)
    {
      byte[] address = BitConverter.GetBytes(reportAddress);
      Data = frameBuilder.BuildLan(0x00A4, type, address[0], address[1]);
    }

    public string Name => "LAN_LOCONET_DETECTOR";

    public byte[] Data { get; }
  }
}
