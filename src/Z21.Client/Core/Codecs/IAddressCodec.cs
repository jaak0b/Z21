using System;

namespace Z21.Core.Codecs
{
  /// <summary>
  /// Encodes and decodes locomotive and accessory addresses in the Z21 wire representation.
  /// </summary>
  public interface IAddressCodec
  {
    (byte lsb, byte msb) SplitLocoAddress(ushort address);

    /// <summary>
    /// Splits an address into its big-endian wire bytes (most-significant byte first), as used by the
    /// LAN_GET/SET_LOCOMODE and LAN_GET/SET_TURNOUTMODE settings commands.
    /// </summary>
    (byte msb, byte lsb) SplitAddressBigEndian(ushort address);

    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="address"/> is smaller than 1.</exception>
    (byte lsb, byte msb) SplitAccessoryAddress(ushort address);

    ushort CombineAccessoryAddress(byte lsb, byte msb);

    /// <summary>
    /// Maps a user-facing extended accessory address (1-based) to its RCN-213 RawAddress wire bytes (user address 1 = RawAddress 4).
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="address"/> is smaller than 1.</exception>
    (byte lsb, byte msb) SplitExtAccessoryAddress(ushort address);

    /// <summary>
    /// Maps the RCN-213 RawAddress wire bytes of an extended accessory decoder back to the user-facing address (RawAddress 4 = user address 1).
    /// </summary>
    ushort CombineExtAccessoryAddress(byte lsb, byte msb);

    /// <summary>
    /// Splits a CV address (0 = CV1) into its high and low wire bytes (no offset applied).
    /// </summary>
    (byte msb, byte lsb) SplitCvAddress(ushort cvAddress);

    /// <summary>
    /// Combines the high and low wire bytes of a CV address back into a CV address (0 = CV1).
    /// </summary>
    ushort CombineCvAddress(byte msb, byte lsb);

    /// <summary>
    /// Encodes an accessory decoder address for POM commands into the two wire bytes
    /// <c>aaaaa</c> / <c>AAAACDDD</c>. When <paramref name="wholeDecoder"/> is true the CV refers to the
    /// whole decoder (<c>CDDD = 0000</c>); otherwise <c>C = 1</c> and <c>DDD = <paramref name="output"/></c>.
    /// </summary>
    (byte db1, byte db2) EncodeAccessoryPomAddress(ushort decoderAddress, bool wholeDecoder, byte output);
  }
}
