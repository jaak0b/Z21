using Z21.Core.Framing;

namespace Z21.Core.Command.Programming
{
  /// <summary>
  /// From Z21 FW version 1.25, reads a register of a DCC decoder in register mode on the programming
  /// track (protocol §6.13). Register range 0x01–0x08.
  /// </summary>
  public class DccReadRegisterCommand : IZ21Command
  {
    public DccReadRegisterCommand(IZ21FrameBuilder frameBuilder, byte register)
    {
      Data = frameBuilder.BuildXBus(0x22, 0x11, register);
    }

    public string Name => "LAN_X_DCC_READ_REGISTER";

    public byte[] Data { get; }
  }
}
