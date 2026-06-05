using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Programming
{
  /// <summary>
  /// From Z21 FW version 1.22, writes a single bit of a CV of an accessory decoder on the main track
  /// (POM, protocol §6.10).
  /// </summary>
  public class CvPomAccessoryWriteBitCommand : IZ21Command
  {
    public CvPomAccessoryWriteBitCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort decoderAddress, bool wholeDecoder, byte output, ushort cvAddress, byte bitPosition, bool bitValue)
    {
      (byte db1, byte db2) = addressCodec.EncodeAccessoryPomAddress(decoderAddress, wholeDecoder, output);
      (byte cvHighBits, byte cvLsb) = addressCodec.SplitPomCvAddress(cvAddress);
      byte db3 = (byte)(0xE8 | cvHighBits);
      // DB5 is the DCC bit-manipulation data byte 1111VPPP (S-9.2.1): high nibble 0xF0 marks a write
      // (the "111K" opcode with K=1), V = new bit value, PPP = bit position.
      byte db5 = (byte)(0xF0 | (bitValue ? 0x08 : 0x00) | (bitPosition & 0x07));
      Data = frameBuilder.BuildXBus(0xE6, 0x31, db1, db2, db3, cvLsb, db5);
    }

    public string Name => "LAN_X_CV_POM_ACCESSORY_WRITE_BIT";

    public byte[] Data { get; }
  }
}
