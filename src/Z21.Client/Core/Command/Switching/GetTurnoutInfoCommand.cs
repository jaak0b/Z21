using System;
using Z21.Core.Codecs;
using Z21.Core.Framing;

namespace Z21.Core.Command.Switching
{
  ///<summary>
  /// The following command can be used to poll the status of a turnout (or any accessory function).
  /// </summary>
  public class GetTurnoutInfoCommand : IZ21Command
  {
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when <paramref name="accessoryAddress"/> is smaller than 1.</exception>
    public GetTurnoutInfoCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ushort accessoryAddress)
    {
      (byte lsb, byte msb) = addressCodec.SplitAccessoryAddress(accessoryAddress);
      Data = frameBuilder.BuildXBus(0x43, msb, lsb);
    }

    public string Name => "LAN_X_GET_TURNOUT_INFO";

    public byte[] Data { get; }
  }
}
