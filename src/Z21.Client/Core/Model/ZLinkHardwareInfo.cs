namespace Z21.Core.Model
{
  /// <summary>
  /// Hardware information of a 10838 Z21 pro LINK adapter (<c>LAN_ZLINK_GET_HWINFO</c> reply, protocol §11.1.1.1).
  /// </summary>
  public record ZLinkHardwareInfo(ushort HardwareId, byte FirmwareMajor, byte FirmwareMinor, ushort FirmwareBuild, string MacAddress, string Name);
}
