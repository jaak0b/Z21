using Z21.Core.Framing;

namespace Z21.Core.Command.Feedback
{
  /// <summary>
  /// Programs the address of an R-BUS feedback module (protocol §7.3). The programming command is issued
  /// on the R-BUS until it is sent again with address 0. Range: 0 and 1–20.
  /// </summary>
  public class ProgramRmBusModuleCommand : IZ21Command
  {
    public ProgramRmBusModuleCommand(IZ21FrameBuilder frameBuilder, byte address)
    {
      Data = frameBuilder.BuildLan(0x0082, address);
    }

    public string Name => "LAN_RMBUS_PROGRAMMODULE";

    public byte[] Data { get; }
  }
}
