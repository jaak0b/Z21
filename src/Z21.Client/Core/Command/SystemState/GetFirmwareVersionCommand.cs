using Z21.Core.Framing;

namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// The firmware version of the Z21 can be read with this command.
  /// </summary>
  public class GetFirmwareVersionCommand : IZ21Command
  {
    public GetFirmwareVersionCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildXBus(0xF1, 0x0A);
    }

    public string Name => "LAN_X_GET_FIRMWARE_VERSION";

    public byte[] Data { get; }
  }
}
