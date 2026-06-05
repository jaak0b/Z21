using System;

namespace Z21.Core.Codecs
{
  public class AddressCodec : IAddressCodec
  {
    public (byte lsb, byte msb) SplitLocoAddress(ushort address)
    {
      byte lsb = (byte)(address & 0xFF);
      byte msb = (byte)((address >> 8) & 0xFF);

      if (address >= 128)
        msb |= 0xC0;

      return (lsb, msb);
    }

    public (byte msb, byte lsb) SplitAddressBigEndian(ushort address)
    {
      byte msb = (byte)((address >> 8) & 0xFF);
      byte lsb = (byte)(address & 0xFF);
      return (msb, lsb);
    }

    public (byte lsb, byte msb) SplitAccessoryAddress(ushort address)
    {
      if (address < 1)
        throw new ArgumentOutOfRangeException(nameof(address), address, "Smallest address is 1");

      ushort dccAddress = (ushort)(address - 1);
      byte msb = (byte)((dccAddress >> 8) & 0xFF);
      byte lsb = (byte)(dccAddress & 0xFF);
      return (lsb, msb);
    }

    public ushort CombineAccessoryAddress(byte lsb, byte msb)
    {
      return (ushort)((msb << 8) + lsb + 1);
    }

    public (byte lsb, byte msb) SplitExtAccessoryAddress(ushort address)
    {
      if (address < 1)
        throw new ArgumentOutOfRangeException(nameof(address), address, "Smallest address is 1");

      ushort rawAddress = (ushort)(address + 3);
      byte msb = (byte)((rawAddress >> 8) & 0xFF);
      byte lsb = (byte)(rawAddress & 0xFF);
      return (lsb, msb);
    }

    public ushort CombineExtAccessoryAddress(byte lsb, byte msb)
    {
      return (ushort)((msb << 8) + lsb - 3);
    }

    public (byte msb, byte lsb) SplitCvAddress(ushort cvAddress)
    {
      byte msb = (byte)((cvAddress >> 8) & 0xFF);
      byte lsb = (byte)(cvAddress & 0xFF);
      return (msb, lsb);
    }

    public ushort CombineCvAddress(byte msb, byte lsb)
    {
      return (ushort)((msb << 8) + lsb);
    }

    public (byte cvHighBits, byte cvLsb) SplitPomCvAddress(ushort cvAddress)
    {
      if (cvAddress > 0x3FF)
        throw new ArgumentOutOfRangeException(nameof(cvAddress), cvAddress, "POM CV address must be between 0 and 1023 (CV1..CV1024); higher CVs are not addressable on the main track.");

      byte cvHighBits = (byte)((cvAddress >> 8) & 0x03);
      byte cvLsb = (byte)(cvAddress & 0xFF);
      return (cvHighBits, cvLsb);
    }

    public (byte db1, byte db2) EncodeAccessoryPomAddress(ushort decoderAddress, bool wholeDecoder, byte output)
    {
      int cddd = wholeDecoder ? 0x00 : (0x08 | (output & 0x07));
      int value = ((decoderAddress & 0x1FF) << 4) | cddd;
      byte db1 = (byte)((value >> 8) & 0xFF);
      byte db2 = (byte)(value & 0xFF);
      return (db1, db2);
    }
  }
}
