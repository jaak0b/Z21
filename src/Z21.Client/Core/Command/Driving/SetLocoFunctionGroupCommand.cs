using Z21.Core.Codecs;
using Z21.Core.Framing;
using Z21.Core.Model;

namespace Z21.Core.Command.Driving
{
  /// <summary>
  /// Switches a whole locomotive function group (up to 8 functions) with a single command (protocol §4.3.2).
  /// </summary>
  public class SetLocoFunctionGroupCommand : IZ21Command
  {
    public SetLocoFunctionGroupCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort locoAddress, LocoFunctionGroup group, byte functions)
    {
      (byte lsb, byte msb) = addressCodec.SplitLocoAddress(locoAddress);
      Data = frameBuilder.BuildXBus(0xE4, (byte)group, msb, lsb, functions);
    }

    public string Name => "LAN_X_SET_LOCO_FUNCTION_GROUP";

    public byte[] Data { get; }
  }
}
