using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Settings
{
  /// <summary>
  /// Read the output format for a given locomotive address.
  /// </summary>
  /// <remarks>
  ///  In the Z21, the output format (DCC, MM) is persistently stored for each locomotive address. A maximum of 256 different locomotive addresses can be stored. Each address >= 256 is DCC automatically.
  /// </remarks>
  public class GetLocoModeCommand : IZ21Command
  {
    public GetLocoModeCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, short locoAddress)
    {
      (byte msb, byte lsb) = addressCodec.SplitAddressBigEndian((ushort)locoAddress);
      Data = frameBuilder.BuildLan(0x0060, msb, lsb);
    }

    public string Name => "LAN_GET_LOCOMODE";

    public byte[] Data { get; }
  }
}
