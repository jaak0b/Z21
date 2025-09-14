using System;

namespace Z21.Core.Helper
{
  public static class AddressHelper
  {
    public static (byte lsb, byte msb) SplitLocoAddress(ushort address)
    {
      byte lsb = (byte)(address & 0xFF);
      byte msb = (byte)((address >> 8) & 0xFF);

      if (address >= 128)
        msb |= 0xC0;

      return (lsb, msb);
    }

    /// <exception cref="ArgumentOutOfRangeException"> Thrown when <paramref name="address"/>> is smaller than 1.</exception>
    public static (byte lsb, byte msb) SplitAccessoryAddress(ushort address)
    {
      if (address < 1)
        throw new ArgumentOutOfRangeException(nameof(address), address, "Smallest address is 1");
      
      ushort dccAddress = (ushort)(address - 1);
      byte msb = (byte)((dccAddress >> 8) & 0xFF);
      byte lsb = (byte)(dccAddress & 0xFF);
      return (lsb, msb);
    }

    public static ushort CombineAccessoryAddress(byte lsb, byte msb)
    {
      return (ushort)((msb << 8) + lsb + 1);
    }
  }
}