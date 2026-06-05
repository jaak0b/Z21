using Z21.Core.Framing;

namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Reading the serial number of the Z21.
  /// </summary>
  public class GetSerialNumberCommand : IZ21Command
  {
    public GetSerialNumberCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildLan(0x0010);
    }

    public string Name => "LAN_GET_SERIAL_NUMBER";

    public byte[] Data { get; }
  }
}
