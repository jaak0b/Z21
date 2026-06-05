using Z21.Core.Framing;

namespace Z21.Core.Command.Programming
{
  /// <summary>
  /// From Z21 FW version 1.25, overwrites a register of a DCC decoder in register mode on the
  /// programming track (protocol §6.14). Register range 0x01–0x08.
  /// </summary>
  public class DccWriteRegisterCommand : IZ21Command
  {
    public DccWriteRegisterCommand(IZ21FrameBuilder frameBuilder, byte register, byte value)
    {
      Data = frameBuilder.BuildXBus(0x23, 0x12, register, value);
    }

    public string Name => "LAN_X_DCC_WRITE_REGISTER";

    public byte[] Data { get; }
  }
}
