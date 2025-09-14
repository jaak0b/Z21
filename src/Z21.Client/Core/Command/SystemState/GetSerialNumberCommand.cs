namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Reading the serial number of the Z21.
  /// </summary>
  public class GetSerialNumberCommand : IZ21Command
  {
    public string Name => "LAN_GET_SERIAL_NUMBER";

    public byte[] Data { get; } =
      [
        0x04,
        0x00,
        0x10,
        0x00
      ];
  }
}