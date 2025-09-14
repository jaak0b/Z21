using System;
using Z21.Core.Model;

namespace Z21.Core.Command.Settings
{
  /// <summary>
  /// Set the output format for a given accessory decoder address. The format is stored in the Z21 persistently.
  /// </summary>
  public class SetAccessoryModeCommand : IZ21Command
  {
    /// <exception cref="ArgumentException">Is thrown when <param name="decoderMode"></param> is <see cref="DecoderMode.Unknown"/></exception>
    public SetAccessoryModeCommand(short accessoryAddress, DecoderMode decoderMode)
    {
      if (decoderMode is DecoderMode.Unknown)
        throw new ArgumentException($"{DecoderMode.Unknown} is not a valid DecoderMode.", nameof(decoderMode));

      byte[] addressBytes = BitConverter.GetBytes(accessoryAddress);
      Array.Reverse(addressBytes);

      Data =
      [
        0x07,
        0x00,
        0x71,
        0x00,
        addressBytes[0],
        addressBytes[1],
        (byte)decoderMode
      ];
    }

    public string Name => "LAN_SET_TURNOUTMODE";

    public byte[] Data { get; }
  }
}