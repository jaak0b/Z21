using System;
using Z21.Core.Model;

namespace Z21.Core.Exception
{
  public class LocoSpeedOutOfRangeException(string message, string parameter) : ArgumentOutOfRangeException(message, parameter)
  {
    public static void ThrowIfOutOfRange(DccSpeedMode dccSpeedMode, ushort locoSpeed)
    {
      switch (dccSpeedMode)
      {
        case DccSpeedMode.Steps14 when locoSpeed > 14:
          throw new LocoSpeedOutOfRangeException($"{nameof(DccSpeedMode.Steps14)} allows for a maximum speed of 14 steps.", nameof(locoSpeed));
        case DccSpeedMode.Steps28 when locoSpeed > 28:
          throw new LocoSpeedOutOfRangeException($"{nameof(DccSpeedMode.Steps28)} allows for a maximum speed of 28 steps.", nameof(locoSpeed));
        case DccSpeedMode.Steps128 when locoSpeed > 126:
          throw new LocoSpeedOutOfRangeException($"{nameof(DccSpeedMode.Steps128)} allows for a maximum speed of 126 steps.", nameof(locoSpeed));
        default:
          return;
      }
    }

  }
}