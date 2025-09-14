using System;
using Z21.Core.Helper;
using Z21.Core.Model;

namespace Z21.Core.Command.Switching
{
  /// <summary>
  /// A turnout (or any accessory function) can be switched with the following command.
  /// </summary>
  /// <remarks>
  /// The <see cref="AccessoryState.Activate"/> signal is output on the track until the LAN client sends the corresponding <see cref="AccessoryState.Deactivate"/>> signale.<para />Only one switching command may be active at the same time.
  /// </remarks>
  /// <example>
  /// Wrong: Activate 1 -> Active 2 -> Deactivate 1 -> Deactivate 2 <para />Correct: Activate 1 -> Deactivate 1 -> Activate 2 -> Deactivate 2
  /// </example>
  public class SetTurnoutCommand : IZ21Command
  {
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when <paramref name="accessoryAddress"/> is smaller than 1.</exception>
    public SetTurnoutCommand(ushort accessoryAddress, AccessoryOutput accessoryOutput, AccessoryState accessoryState, bool executeImmediately)
    {
      (byte lsb, byte msb) = AddressHelper.SplitAccessoryAddress(accessoryAddress);
      byte db2 = (byte)(0x80 | (int)accessoryOutput | (int)accessoryState | (executeImmediately ? 0x20 : 0x00));

      Data =
      [
        0x09, 0x00,
        0x40, 0x00,
        0x53,
        msb,
        lsb,
        db2,
        (byte)(0x53 ^ msb ^ lsb ^ db2)
      ];
    }

    public string Name => "LAN_X_SET_TURNOUT";

    public byte[] Data { get; }
  }
}