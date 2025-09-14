namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// The firmware version of the Z21 can be read with this command.
  /// </summary>
  public class GetFirmwareVersionCommand : IZ21Command
  {
    public string Name => "LAN_X_GET_FIRMWARE_VERSION";

    public byte[] Data { get; } =
      [
        0x07,
        0x00,
        0x40,
        0x00,
        0xF1,
        0x0A,
        0xF1 ^ 0x0A
      ];
  }
}