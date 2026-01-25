namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// This command can be used to request the Z21 status.
  /// </summary>
  public class GetStatusCommand : IZ21Command
  {
    public string Name => "LAN_X_GET_STATUS";

    public byte[] Data { get; } =
      [
        0x07, 0x00, //DataLen
        0x40, 0x00, //Header
        0x21, // X-Header
        0x24, // DB0
        0x21 ^ 0x24 // XOR-Byte
      ];
  }
}