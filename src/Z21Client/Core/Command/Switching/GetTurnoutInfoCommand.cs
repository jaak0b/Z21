using System;
using Z21.Core.Helper;

namespace Z21.Core.Command.Switching
{
  ///<summary>
  /// The following command can be used to poll the status of a turnout (or any accessory function).
  /// </summary>
  public class GetTurnoutInfoCommand : IZ21Command
  {
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when <paramref name="accessoryAddress"/> is smaller than 1.</exception>
    public GetTurnoutInfoCommand(ushort accessoryAddress)
    {
      (byte lsb, byte msb) = AddressHelper.SplitAccessoryAddress(accessoryAddress);
      Data =
      [
        0x08, 0x00,
        0x40, 0x00,
        0x43,
        msb,
        lsb,
        (byte)(0x43 ^ msb ^ lsb)
      ];
    }

    public string Name => "LAN_X_GET_TURNOUT_INFO";

    public byte[] Data { get; }
  }
}