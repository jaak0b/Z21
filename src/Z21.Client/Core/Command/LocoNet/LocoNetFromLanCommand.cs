using Z21.Core.Framing;

namespace Z21.Core.Command.LocoNet
{
  /// <summary>
  /// From Z21 FW version 1.20, writes a raw LocoNet message (including its checksum) onto the LocoNet bus
  /// (<c>LAN_LOCONET_FROM_LAN</c>, protocol §9.3).
  /// </summary>
  public class LocoNetFromLanCommand : IZ21Command
  {
    public LocoNetFromLanCommand(IZ21FrameBuilder frameBuilder, byte[] message)
    {
      Data = frameBuilder.BuildLan(0x00A2, message);
    }

    public string Name => "LAN_LOCONET_FROM_LAN";

    public byte[] Data { get; }
  }
}
