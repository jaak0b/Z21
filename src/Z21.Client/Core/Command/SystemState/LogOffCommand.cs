namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Logging off the client from the Z21. 
  /// </summary>
  public class LogOffCommand : IZ21Command
  {
    public string Name => "LAN_LOGOFF";

    public byte[] Data { get; } =
      [
        0x04,
        0x00,
        0x30,
        0x00
      ];
  }
}