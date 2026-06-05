using Z21.Core.Framing;

namespace Z21.Core.Command.Decoder
{
  /// <summary>
  /// Requests the system state of a zLink decoder (<c>LAN_DECODER_SYSTEMSTATE_GETDATA</c>, protocol §11.3.3).
  /// </summary>
  public class GetDecoderSystemStateCommand : IZ21Command
  {
    public GetDecoderSystemStateCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildLan(0x00DB);
    }

    public string Name => "LAN_DECODER_SYSTEMSTATE_GETDATA";

    public byte[] Data { get; }
  }
}
