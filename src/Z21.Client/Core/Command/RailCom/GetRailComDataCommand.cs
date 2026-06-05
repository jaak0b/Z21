using System;
using Z21.Core.Framing;

namespace Z21.Core.Command.RailCom
{
  /// <summary>
  /// From Z21 FW version 1.29, requests RailCom data for a given locomotive address (protocol §8.2).
  /// Locomotive address 0 returns the next locomotive in the ring buffer.
  /// </summary>
  public class GetRailComDataCommand : IZ21Command
  {
    public GetRailComDataCommand(IZ21FrameBuilder frameBuilder, ushort locoAddress)
    {
      byte[] address = BitConverter.GetBytes(locoAddress);
      Data = frameBuilder.BuildLan(0x0089, 0x01, address[0], address[1]);
    }

    public string Name => "LAN_RAILCOM_GETDATA";

    public byte[] Data { get; }
  }
}
