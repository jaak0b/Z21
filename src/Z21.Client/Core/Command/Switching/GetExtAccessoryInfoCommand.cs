using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Switching
{
  /// <summary>
  /// From Z21 FW V1. 40, the following request can be used to poll the last command transferred to an extended accessory decoder.
  /// </summary>
  public class GetExtAccessoryInfoCommand : IZ21Command
  {
    public GetExtAccessoryInfoCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort accessoryAddress)
    {
      (byte lsb, byte msb) = addressCodec.SplitExtAccessoryAddress(accessoryAddress);
      Data = frameBuilder.BuildXBus(0x44, msb, lsb, 0x00);
    }

    public string Name => "LAN_X_GET_EXT_ACCESSORY_INFO";

    public byte[] Data { get; }
  }
}
