using System;
using System.Collections.Generic;
using Z21.Core.Model;

namespace Z21.Core.Helper
{
  public class LocoSpeedHelper
  {
    /// <summary>
    /// Calculates the dcc step that should send to the Z21.
    /// </summary>
    /// <param name="dccSpeedMode"></param>
    /// <param name="speedStep"></param>
    /// <returns></returns>
    public static ushort CalculateDccSpeed(DccSpeedMode dccSpeedMode, ushort speedStep) => dccSpeedMode switch
                                                                                           {
                                                                                             DccSpeedMode.Steps14 when speedStep > 0 => (ushort)(speedStep + 1),
                                                                                             DccSpeedMode.Steps28 when speedStep > 0 => CalculateDcc28DccSpeed(speedStep + 3),
                                                                                             DccSpeedMode.Steps128 when speedStep > 0 => (ushort)(speedStep + 1),
                                                                                             _ => speedStep
                                                                                           };

    private static ushort CalculateDcc28DccSpeed(int speedStep)
    {
      double dcc14Speed = speedStep / 2.0;
      int dccSpeed = (int)Math.Floor(dcc14Speed);

      if (dcc14Speed % 1 != 0)
        dccSpeed |= 0x10;
      return (ushort)dccSpeed;
    }

    /// <summary>
    /// Calculates the dcc speed step that will be sent to event subscribers. 
    /// </summary>
    /// <param name="dccSpeedMode"></param>
    /// <param name="dccSpeed"></param>
    /// <returns></returns>
    public static ushort CalculateSpeedStep(DccSpeedMode dccSpeedMode, ushort dccSpeed) => dccSpeedMode switch
                                                                                           {
                                                                                             DccSpeedMode.Steps14 when dccSpeed > 1 => (ushort)(dccSpeed - 1),
                                                                                             DccSpeedMode.Steps28 when dccSpeed > 0 => CalculateDcc28SpeedStep(dccSpeed),
                                                                                             DccSpeedMode.Steps128 when dccSpeed > 1 => (ushort)(dccSpeed - 1),
                                                                                             _ => 0
                                                                                           };

    private readonly static Dictionary<ushort, ushort> CalculateDcc28SpeedStepLookup = new()
                                                                                       {
                                                                                         { 0, 0 }, { 16, 0 },
                                                                                         { 1, 0 }, { 17, 0 },
                                                                                         { 2, 1 }, { 18, 2 },
                                                                                         { 3, 3 }, { 19, 4 },
                                                                                         { 4, 5 }, { 20, 6 },
                                                                                         { 5, 7 }, { 21, 8 },
                                                                                         { 6, 9 }, { 22, 10 },
                                                                                         { 7, 11 }, { 23, 12 },
                                                                                         { 8, 13 }, { 24, 14 },
                                                                                         { 9, 15 }, { 25, 16 },
                                                                                         { 10, 17 }, { 26, 18 },
                                                                                         { 11, 19 }, { 27, 20 },
                                                                                         { 12, 21 }, { 28, 22 },
                                                                                         { 13, 23 }, { 29, 24 },
                                                                                         { 14, 25 }, { 30, 26 },
                                                                                         { 15, 27 }, { 31, 28 }
                                                                                       };


    private static ushort CalculateDcc28SpeedStep(ushort dccSpeed)
    {
      return CalculateDcc28SpeedStepLookup[dccSpeed];
    }
  }
}