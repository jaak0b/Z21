using Z21.Core.Framing;

namespace Z21.Core.Command.Programming
{
  /// <summary>
  /// From Z21 FW version 1.23, overwrites a register of a Motorola decoder on the programming track
  /// (protocol §6.12). Register range 0–78.
  /// </summary>
  public class MmWriteByteCommand : IZ21Command
  {
    public MmWriteByteCommand(IZ21FrameBuilder frameBuilder, byte register, byte value)
    {
      Data = frameBuilder.BuildXBus(0x24, 0xFF, 0x00, register, value);
    }

    public string Name => "LAN_X_MM_WRITE_BYTE";

    public byte[] Data { get; }
  }
}
