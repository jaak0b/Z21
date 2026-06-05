using Z21.Core.Framing;

namespace Z21.Core.Command.Booster
{
  /// <summary>
  /// Requests the system state of a zLink booster (<c>LAN_BOOSTER_SYSTEMSTATE_GETDATA</c>, protocol §11.2.3).
  /// </summary>
  public class GetBoosterSystemStateCommand : IZ21Command
  {
    public GetBoosterSystemStateCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildLan(0x00BB);
    }

    public string Name => "LAN_BOOSTER_SYSTEMSTATE_GETDATA";

    public byte[] Data { get; }
  }
}
