using Z21.Core.Helper;

namespace Z21.Core.Command.Switching
{
  /// <summary>
  /// From Z21 FW V1. 40, the following request can be used to poll the last command transferred to an extended accessory decoder.
  /// </summary>
  public class GetExtAccessoryInfoCommand : IZ21Command
  {
    public GetExtAccessoryInfoCommand(ushort accessoryAddress)
    {
      (byte lsb, byte msb) = AddressHelper.SplitAccessoryAddress(accessoryAddress);
      Data =
      [
        0x09, 0x00,
        0x40, 0x00,
        0x44,
        msb,
        lsb,
        0x00,
        (byte)(0x44 ^ msb ^ lsb ^ 0x00)
      ];
    }

    public string Name => "LAN_X_GET_EXT_ACCESSORY_INFO";

    public byte[] Data { get; }
  }
}