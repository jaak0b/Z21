using Z21.Core.Model;

namespace Z21.Core.Codecs
{
  /// <summary>
  /// Converts between user-facing DCC speed steps and the speed bytes used on the Z21 wire.
  /// </summary>
  public interface ILocoSpeedCodec
  {
    ushort CalculateDccSpeed(DccSpeedMode dccSpeedMode, ushort speedStep);

    ushort CalculateSpeedStep(DccSpeedMode dccSpeedMode, ushort dccSpeed);
  }
}
