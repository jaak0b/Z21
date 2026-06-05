using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Programming
{
  /// <summary>
  /// From Z21 FW version 1.22, writes a CV of an accessory decoder on the main track (POM, protocol §6.9).
  /// When <c>wholeDecoder</c> is true the CV refers to the whole decoder; otherwise to a single output.
  /// </summary>
  public class CvPomAccessoryWriteByteCommand : IZ21Command
  {
    public CvPomAccessoryWriteByteCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort decoderAddress, bool wholeDecoder, byte output, ushort cvAddress, byte value)
    {
      (byte db1, byte db2) = addressCodec.EncodeAccessoryPomAddress(decoderAddress, wholeDecoder, output);
      byte db3 = (byte)(0xEC | ((cvAddress >> 8) & 0x03));
      byte cvLsb = (byte)(cvAddress & 0xFF);
      Data = frameBuilder.BuildXBus(0xE6, 0x31, db1, db2, db3, cvLsb, value);
    }

    public string Name => "LAN_X_CV_POM_ACCESSORY_WRITE_BYTE";

    public byte[] Data { get; }
  }
}
