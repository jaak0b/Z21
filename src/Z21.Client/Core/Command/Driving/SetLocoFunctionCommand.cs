using Z21.Core.Helper;
using Z21.Core.Model;

namespace Z21.Core.Command.Driving
{
  /// <summary>
  /// Change a function of a locomotive.
  /// </summary>
  public class SetLocoFunctionCommand : IZ21Command
  {
    public SetLocoFunctionCommand(ushort locoAddress, ushort functionIndex, FunctionToggleType toggleType)
    {
      const byte xHeader = 0xE4;
      const byte db0 = 0xF8;
      byte db3 = (byte)((byte)toggleType | functionIndex);
      (byte lsb, byte msb) = AddressHelper.SplitLocoAddress(locoAddress);
      Data =
      [
        0x0A, 0x00,
        0x40, 0x00,
        xHeader,
        db0,
        msb,
        lsb,
        db3,
        (byte)(xHeader ^ db0 ^ msb ^ lsb ^ db3)
      ];
    }

    public string Name => "LAN_X_SET_LOCO_FUNCTION";

    public byte[] Data { get; }
  }
}