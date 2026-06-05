using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Settings
{
  /// <summary>
  /// Read the settings for a given accessory decoder address ("Accessory Decoder" RP-9.2.1).
  /// </summary>
  /// <remarks>
  /// In the Z21, the output format (DCC, MM) is persistently stored for each accessory decoder address.
  /// A maximum of 256 different accessory decoder addresses can be stored. Each address >= 256 is automatically DCC.
  /// </remarks>
  public class GetAccessoryModeCommand : IZ21Command
  {
    public GetAccessoryModeCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, short accessoryAddress)
    {
      (byte msb, byte lsb) = addressCodec.SplitAddressBigEndian((ushort)accessoryAddress);
      Data = frameBuilder.BuildLan(0x0070, msb, lsb);
    }

    public string Name => "LAN_GET_TURNOUTMODE";

    public byte[] Data { get; }
  }
}
