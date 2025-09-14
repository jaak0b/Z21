using Z21.Core.Command.SystemState;
using Z21.Core.Helper;
using Z21.Core.Model;

namespace Z21.Core.Command.Driving
{
  /// <summary>
  /// The following command can be used to poll the status of a locomotive. At the same time, the client also "subscribes" to the locomotive information for this locomotive address (only in combination with <see cref="SetBroadcastFlagsCommand"/>, Flag <see cref="Z21BroadcastFlags.DriveAndSwitchingMessages"/>).
  /// </summary>
  public class GetLocoInfoCommand : IZ21Command
  {
    public GetLocoInfoCommand(ushort locoAddress)
    {
      const byte xHeader = 0xE3;
      const byte db0 = 0xF0;

      (byte lsb, byte msb) = AddressHelper.SplitLocoAddress(locoAddress);
      byte xor = (byte)(xHeader ^ db0 ^ lsb ^ msb);
      Data =
      [
        0x09, 0x00,
        0x40, 0x00,
        xHeader,
        db0,
        msb,
        lsb,
        xor
      ];
    }

    public string Name => "LAN_X_GET_LOCO_INFO";

    public byte[] Data { get; }
  }
}