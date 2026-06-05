using Z21.Core.Framing;

namespace Z21.Core.Command.Booster
{
  /// <summary>
  /// Reads the description of a zLink booster (<c>LAN_BOOSTER_GET_DESCRIPTION</c>, protocol §11.2.1).
  /// </summary>
  public class GetBoosterDescriptionCommand : IZ21Command
  {
    public GetBoosterDescriptionCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildLan(0x00B8);
    }

    public string Name => "LAN_BOOSTER_GET_DESCRIPTION";

    public byte[] Data { get; }
  }
}
