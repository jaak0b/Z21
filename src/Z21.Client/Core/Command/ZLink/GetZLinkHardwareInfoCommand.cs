using Z21.Core.Framing;

namespace Z21.Core.Command.ZLink
{
  /// <summary>
  /// Queries the properties of a 10838 Z21 pro LINK adapter (<c>LAN_ZLINK_GET_HWINFO</c>, protocol §11.1.1.1).
  /// </summary>
  public class GetZLinkHardwareInfoCommand : IZ21Command
  {
    public GetZLinkHardwareInfoCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildLan(0x00E8, 0x06);
    }

    public string Name => "LAN_ZLINK_GET_HWINFO";

    public byte[] Data { get; }
  }
}
