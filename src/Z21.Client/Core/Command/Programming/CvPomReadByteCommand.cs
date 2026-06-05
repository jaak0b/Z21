using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Programming
{
  /// <summary>
  /// From Z21 FW version 1.22, reads a CV of a locomotive decoder on the main track (POM, protocol §6.8).
  /// Requires RailCom enabled. CV address 0 = CV1.
  /// </summary>
  public class CvPomReadByteCommand : IZ21Command
  {
    public CvPomReadByteCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort locoAddress, ushort cvAddress)
    {
      (byte lsb, byte msb) = addressCodec.SplitLocoAddress(locoAddress);
      byte db3 = (byte)(0xE4 | ((cvAddress >> 8) & 0x03));
      byte cvLsb = (byte)(cvAddress & 0xFF);
      Data = frameBuilder.BuildXBus(0xE6, 0x30, msb, lsb, db3, cvLsb, 0x00);
    }

    public string Name => "LAN_X_CV_POM_READ_BYTE";

    public byte[] Data { get; }
  }
}
