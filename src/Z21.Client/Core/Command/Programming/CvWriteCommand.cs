using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Programming
{
  /// <summary>
  /// Overwrites a CV in direct mode on the programming track (protocol §6.2). CV address 0 = CV1.
  /// </summary>
  public class CvWriteCommand : IZ21Command
  {
    public CvWriteCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort cvAddress, byte value)
    {
      (byte msb, byte lsb) = addressCodec.SplitCvAddress(cvAddress);
      Data = frameBuilder.BuildXBus(0x24, 0x12, msb, lsb, value);
    }

    public string Name => "LAN_X_CV_WRITE";

    public byte[] Data { get; }
  }
}
