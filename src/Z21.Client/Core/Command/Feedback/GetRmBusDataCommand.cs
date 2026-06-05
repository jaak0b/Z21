using Z21.Core.Framing;

namespace Z21.Core.Command.Feedback
{
  /// <summary>
  /// Requests the current status of the R-BUS feedback modules (protocol §7.2). Group index 0 covers
  /// modules with addresses 1–10, group index 1 covers addresses 11–20.
  /// </summary>
  public class GetRmBusDataCommand : IZ21Command
  {
    public GetRmBusDataCommand(IZ21FrameBuilder frameBuilder, byte groupIndex)
    {
      Data = frameBuilder.BuildLan(0x0081, groupIndex);
    }

    public string Name => "LAN_RMBUS_GETDATA";

    public byte[] Data { get; }
  }
}
