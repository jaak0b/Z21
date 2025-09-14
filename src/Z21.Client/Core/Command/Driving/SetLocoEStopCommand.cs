using Z21.Core.Helper;

namespace Z21.Core.Command.Driving
{
  /// <summary>
  /// From Z21 FW version 1.43, a locomotive can be stopped with the following command.
  /// In the case of a DCC locomotive, the speed step "E-STOP" ("emergency stop" according to RCN-212) is then sent in the DCC speed command onto the track, i.e., the decoder should stop the engine as quickly as possible.
  /// In the case of an MM locomotive, the speed step 0 ("Stop") is sent onto the track.
  /// </summary>
  public class SetLocoEStopCommand : IZ21Command
  {
    public SetLocoEStopCommand(ushort locoAddress)
    {
      (byte lsb, byte msb) = AddressHelper.SplitLocoAddress(locoAddress);
      Data =
      [
        0x08,
        0x00,
        0x40,
        0x00,
        0x92,
        msb,
        lsb,
        (byte)(0x92 ^ msb ^ lsb)
      ];
    }

    public string Name => "LAN_X_SET_LOCO_E_STOP";

    public byte[] Data { get; }
  }
}