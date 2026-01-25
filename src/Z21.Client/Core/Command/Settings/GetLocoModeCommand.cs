using System;

namespace Z21.Core.Command.Settings
{
  /// <summary>
  /// Read the output format for a given locomotive address.
  /// </summary>
  /// <remarks>
  ///  In the Z21, the output format (DCC, MM) is persistently stored for each locomotive address. A maximum of 256 different locomotive addresses can be stored. Each address >= 256 is DCC automatically.
  /// </remarks>
  public class GetLocoModeCommand : IZ21Command
  {

    public GetLocoModeCommand(short locoAddress)
    {
      byte[] addressBytes = BitConverter.GetBytes(locoAddress);
      Array.Reverse(addressBytes);

      Data =
      [
        0x06,
        0x00,
        0x60,
        0x00,
        addressBytes[0],
        addressBytes[1]
      ];
    }

    public string Name => "LAN_GET_LOCOMODE";

    public byte[] Data { get; }
  }
}