namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Read the software feature scope of the Z21 (and z21 or z21start of course).
  /// </summary>
  /// <remarks>This command is of particular interest for the hardware variant "z21 start", in order to be able to check whether driving and switching via LAN is blocked or permitted.</remarks>
  public class GetSoftwareLockCommand : IZ21Command
  {
    public string Name => "LAN_GET_CODE";

    public byte[] Data { get; } =
      [
        0x04,
        0x00,
        0x18,
        0x00
      ];
  }
}