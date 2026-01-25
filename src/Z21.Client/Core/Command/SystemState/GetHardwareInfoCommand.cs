namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Read the hardware type and the firmware version of the Z21.
  /// </summary>
  public class GetHardwareInfoCommand : IZ21Command
  {
    public string Name => "LAN_GET_HWINFO";

    public byte[] Data { get; } =
      [
        0x04,
        0x00,
        0x1A,
        0x00
      ];
  }
}