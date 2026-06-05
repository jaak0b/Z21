using System;
using Z21.Core.Codecs;
using Z21.Core.Framing;
using Z21.Core.Model;

namespace Z21.Core.Command.Settings
{
  /// <summary>
  /// Set the output format for a given locomotive address. The format is stored in the Z21persistently.
  /// </summary>
  public class SetLocoModeCommand : IZ21Command
  {
    /// <exception cref="ArgumentException">Is thrown when <param name="decoderMode"></param> is <see cref="DecoderMode.Unknown"/></exception>
    public SetLocoModeCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, short locoAddress, DecoderMode decoderMode)
    {
      if (decoderMode is DecoderMode.Unknown)
        throw new ArgumentException($"{DecoderMode.Unknown} is not a valid DecoderMode.", nameof(decoderMode));

      (byte msb, byte lsb) = addressCodec.SplitAddressBigEndian((ushort)locoAddress);
      Data = frameBuilder.BuildLan(0x0061, msb, lsb, (byte)decoderMode);
    }

    public string Name => "LAN_SET_LOCOMODE";

    public byte[] Data { get; }
  }
}
