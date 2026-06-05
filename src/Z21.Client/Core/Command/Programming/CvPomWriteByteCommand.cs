using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Programming
{
  /// <summary>
  /// Writes a CV of a locomotive decoder on the main track (POM, protocol §6.6). CV address 0 = CV1.
  /// </summary>
  public class CvPomWriteByteCommand : IZ21Command
  {
    public CvPomWriteByteCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort locoAddress, ushort cvAddress, byte value)
    {
      (byte lsb, byte msb) = addressCodec.SplitLocoAddress(locoAddress);
      byte db3 = (byte)(0xEC | ((cvAddress >> 8) & 0x03));
      byte cvLsb = (byte)(cvAddress & 0xFF);
      Data = frameBuilder.BuildXBus(0xE6, 0x30, msb, lsb, db3, cvLsb, value);
    }

    public string Name => "LAN_X_CV_POM_WRITE_BYTE";

    public byte[] Data { get; }
  }
}
