namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Reading the broadcast flags in the Z21.
  /// </summary>
  public class GetBroadcastFlagsCommand : IZ21Command
  {
    public string Name => "LAN_GET_BROADCASTFLAGS";

    public byte[] Data { get; } =
      [
        0x04,
        0x00,
        0x51,
        0x00
      ];
  }
}