using System;
using Z21.Core.Model;

namespace Z21.Core.Command.Settings
{
  /// <summary>
  /// Set the output format for a given locomotive address. The format is stored in the Z21persistently.
  /// </summary>
  public class SetLocoModeCommand : IZ21Command
  {
    /// <exception cref="ArgumentException">Is thrown when <param name="decoderMode"></param> is <see cref="DecoderMode.Unknown"/></exception>
    public SetLocoModeCommand(short locoAddress, DecoderMode decoderMode)
    {
      if (decoderMode is DecoderMode.Unknown)
        throw new ArgumentException($"{DecoderMode.Unknown} is not a valid DecoderMode.", nameof(decoderMode));

      byte[] addressBytes = BitConverter.GetBytes(locoAddress);
      Array.Reverse(addressBytes);

      Data =
      [
        0x07,
        0x00,
        0x61,
        0x00,
        addressBytes[0],
        addressBytes[1],
        (byte)decoderMode
      ];
    }

    public string Name => "LAN_SET_LOCOMODE";

    public byte[] Data { get; }
  }
}