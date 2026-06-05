using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Programming
{
  /// <summary>
  /// From Z21 FW version 1.22, reads a CV of an accessory decoder on the main track (POM, protocol §6.11).
  /// Requires RailCom enabled.
  /// </summary>
  public class CvPomAccessoryReadByteCommand : IZ21Command
  {
    public CvPomAccessoryReadByteCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort decoderAddress, bool wholeDecoder, byte output, ushort cvAddress)
    {
      (byte db1, byte db2) = addressCodec.EncodeAccessoryPomAddress(decoderAddress, wholeDecoder, output);
      (byte cvHighBits, byte cvLsb) = addressCodec.SplitPomCvAddress(cvAddress);
      byte db3 = (byte)(0xE4 | cvHighBits);
      Data = frameBuilder.BuildXBus(0xE6, 0x31, db1, db2, db3, cvLsb, 0x00);
    }

    public string Name => "LAN_X_CV_POM_ACCESSORY_READ_BYTE";

    public byte[] Data { get; }
  }
}
