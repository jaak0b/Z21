using Z21.Core.Framing;

namespace Z21.Core.Command.Decoder
{
  /// <summary>
  /// Reads the description of a zLink decoder (<c>LAN_DECODER_GET_DESCRIPTION</c>, protocol §11.3.1).
  /// </summary>
  public class GetDecoderDescriptionCommand : IZ21Command
  {
    public GetDecoderDescriptionCommand(IZ21FrameBuilder frameBuilder)
    {
      Data = frameBuilder.BuildLan(0x00D8);
    }

    public string Name => "LAN_DECODER_GET_DESCRIPTION";

    public byte[] Data { get; }
  }
}
