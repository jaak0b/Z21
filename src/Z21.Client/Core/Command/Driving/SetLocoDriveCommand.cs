using System;
using Z21.Core.Codecs;
using Z21.Core.Exception;
using Z21.Core.Framing;
using Z21.Core.Model;

namespace Z21.Core.Command.Driving
{
  /// <summary>
  /// Change the speed and direction of a locomotive.
  /// </summary>
  public class SetLocoDriveCommand : IZ21Command
  {
    public SetLocoDriveCommand(IZ21FrameBuilder frameBuilder, IAddressCodec addressCodec, ILocoSpeedCodec locoSpeedCodec, DccSpeedMode dccSpeedMode, ushort locoAddress, DrivingDirection drivingDirection, ushort locoSpeed)
    {
      LocoSpeedOutOfRangeException.ThrowIfOutOfRange(dccSpeedMode, locoSpeed);
      ushort dccSpeed = locoSpeedCodec.CalculateDccSpeed(dccSpeedMode, locoSpeed);

      byte db0 = (byte)(0x10 | GetByte(dccSpeedMode));
      (byte lsb, byte msb) = addressCodec.SplitLocoAddress(locoAddress);
      byte db3 = (byte)((byte)drivingDirection | dccSpeed);
      Data = frameBuilder.BuildXBus(0xE4, db0, msb, lsb, db3);
    }

    public string Name => "LAN_X_SET_LOCO_DRIVE";

    public byte[] Data { get; }

    private byte GetByte(DccSpeedMode dccSpeedMode) => dccSpeedMode switch
                                                       {
                                                         DccSpeedMode.Steps14 => 0x0,
                                                         DccSpeedMode.Steps28 => 0x02,
                                                         DccSpeedMode.Steps128 => 0x03,
                                                         _ => throw new ArgumentOutOfRangeException(nameof(dccSpeedMode), dccSpeedMode, null)
                                                       };
  }
}
