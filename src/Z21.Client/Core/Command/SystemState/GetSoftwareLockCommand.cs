using Z21.Core.Framing;

namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Read the software feature scope of the Z21 (and z21 or z21start of course).
  /// </summary>
  /// <remarks>This command is of particular interest for the hardware variant "z21 start", in order to be able to check whether driving and switching via LAN is blocked or permitted.</remarks>
  public class GetSoftwareLockCommand : IZ21Command
  {
    public GetSoftwareLockCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildLan(0x0018);
    }

    public string Name => "LAN_GET_CODE";

    public byte[] Data { get; }
  }
}
