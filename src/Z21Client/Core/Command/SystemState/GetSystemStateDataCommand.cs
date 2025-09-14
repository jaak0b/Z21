namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Request the current system status.
  /// </summary>
  public class GetSystemStateDataCommand : IZ21Command
  {
    public string Name => "LAN_SYSTEMSTATE_GETDATA";

    public byte[] Data { get; } =
      [
        0x04,
        0x00,
        0x85,
        0x00
      ];
  }
}