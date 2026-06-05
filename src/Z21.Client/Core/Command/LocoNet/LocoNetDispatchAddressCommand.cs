using System;
using Z21.Core.Framing;

namespace Z21.Core.Command.LocoNet
{
  /// <summary>
  /// From Z21 FW version 1.20, prepares a locomotive address for LocoNet dispatch ("DISPATCH_PUT",
  /// <c>LAN_LOCONET_DISPATCH_ADDR</c>, protocol §9.4).
  /// </summary>
  public class LocoNetDispatchAddressCommand : IZ21Command
  {
    public LocoNetDispatchAddressCommand(IZ21FrameBuilder frameBuilder, ushort locoAddress)
    {
      byte[] address = BitConverter.GetBytes(locoAddress);
      Data = frameBuilder.BuildLan(0x00A3, address[0], address[1]);
    }

    public string Name => "LAN_LOCONET_DISPATCH_ADDR";

    public byte[] Data { get; }
  }
}
