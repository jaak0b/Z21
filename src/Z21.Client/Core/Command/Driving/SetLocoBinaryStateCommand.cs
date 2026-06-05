using System;
using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Driving
{
  /// <summary>
  /// From Z21 FW version 1.42, sends a DCC "binary state" command to a locomotive decoder (protocol §4.3.3).
  /// Allowed binary state addresses are 29 to 32767.
  /// </summary>
  public class SetLocoBinaryStateCommand : IZ21Command
  {
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="binaryStateAddress"/> is outside 29..32767.</exception>
    public SetLocoBinaryStateCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort locoAddress, ushort binaryStateAddress, bool enabled)
    {
      if (binaryStateAddress is < 29 or > 32767)
        throw new ArgumentOutOfRangeException(nameof(binaryStateAddress), binaryStateAddress, "Binary state address must be between 29 and 32767.");

      (byte lsb, byte msb) = addressCodec.SplitLocoAddress(locoAddress);
      byte db3 = (byte)((enabled ? 0x80 : 0x00) | (binaryStateAddress & 0x7F));
      byte db4 = (byte)((binaryStateAddress >> 7) & 0xFF);
      Data = frameBuilder.BuildXBus(0xE5, 0x5F, msb, lsb, db3, db4);
    }

    public string Name => "LAN_X_SET_LOCO_BINARY_STATE";

    public byte[] Data { get; }
  }
}
