using Z21.Core.Framing;

namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Read the hardware type and the firmware version of the Z21.
  /// </summary>
  public class GetHardwareInfoCommand : IZ21Command
  {
    public GetHardwareInfoCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildLan(0x001A);
    }

    public string Name => "LAN_GET_HWINFO";

    public byte[] Data { get; }
  }
}
