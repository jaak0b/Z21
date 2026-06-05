using System;
using Z21.Core.Codecs;
using Z21.Core.Framing;
using Z21.Core.Model;

namespace Z21.Core.Command.Driving
{
  /// <summary>
  /// Change a function of a locomotive.
  /// </summary>
  public class SetLocoFunctionCommand : IZ21Command
  {
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="functionIndex"/> exceeds the 6-bit field (0..63).</exception>
    public SetLocoFunctionCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort locoAddress, ushort functionIndex, FunctionToggleType toggleType)
    {
      if (functionIndex > 0x3F)
        throw new ArgumentOutOfRangeException(nameof(functionIndex), functionIndex, "Function index must be between 0 and 63 (the 6-bit NNNNNN field of LAN_X_SET_LOCO_FUNCTION).");

      byte db3 = (byte)((byte)toggleType | functionIndex);
      (byte lsb, byte msb) = addressCodec.SplitLocoAddress(locoAddress);
      Data = frameBuilder.BuildXBus(0xE4, 0xF8, msb, lsb, db3);
    }

    public string Name => "LAN_X_SET_LOCO_FUNCTION";

    public byte[] Data { get; }
  }
}
