namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// The X-Bus version of the Z21 can be read out with the following command.
  /// </summary>
  public class GetVersionCommand : IZ21Command
  {
    public string Name => "LAN_X_GET_VERSION";

    public byte[] Data { get; } =
      [
        0x07, 0x00,
        0x40, 0x00,
        0x21,
        0x21,
        0x21 ^ 0x21
      ];
  }
}