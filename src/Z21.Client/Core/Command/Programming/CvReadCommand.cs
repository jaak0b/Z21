using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Programming
{
  /// <summary>
  /// Reads a CV in direct mode on the programming track (protocol §6.1). CV address 0 = CV1.
  /// </summary>
  public class CvReadCommand : IZ21Command
  {
    public CvReadCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort cvAddress)
    {
      (byte msb, byte lsb) = addressCodec.SplitCvAddress(cvAddress);
      Data = frameBuilder.BuildXBus(0x23, 0x11, msb, lsb);
    }

    public string Name => "LAN_X_CV_READ";

    public byte[] Data { get; }
  }
}
