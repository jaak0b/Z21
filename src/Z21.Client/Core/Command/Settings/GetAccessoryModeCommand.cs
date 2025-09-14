using System;

namespace Z21.Core.Command.Settings
{
  /// <summary>
  /// Read the settings for a given accessory decoder address ("Accessory Decoder" RP-9.2.1). 
  /// </summary>
  /// <remarks>
  /// In the Z21, the output format (DCC, MM) is persistently stored for each accessory decoder address.
  /// A maximum of 256 different accessory decoder addresses can be stored. Each address >= 256 is automatically DCC.
  /// </remarks>
  public class GetAccessoryModeCommand : IZ21Command
  {
    public GetAccessoryModeCommand(short locoAddress)
    {
      byte[] addressBytes = BitConverter.GetBytes(locoAddress);
      Array.Reverse(addressBytes);

      Data =
      [
        0x06,
        0x00,
        0x70,
        0x00,
        addressBytes[0],
        addressBytes[1]
      ];
    }

    public string Name => "LAN_GET_TURNOUTMODE";

    public byte[] Data { get; }
  }
}