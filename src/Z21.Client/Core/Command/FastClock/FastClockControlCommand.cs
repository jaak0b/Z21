using System;
using CommandStation.Model;
using Z21.Core.Framing;
using Z21.Core.Model;

namespace Z21.Core.Command.FastClock
{
  /// <summary>
  /// From Z21 FW version 1.43, reads, sets, starts or stops the model time
  /// (<c>LAN_FAST_CLOCK_CONTROL</c>, protocol §12.1).
  /// </summary>
  public class FastClockControlCommand : IZ21Command
  {
    public FastClockControlCommand(IZ21FrameBuilder frameBuilder, FastClockAction action)
    {
      byte selector = action switch
                      {
                        FastClockAction.Start => 0x2C,
                        FastClockAction.Stop => 0x2D,
                        _ => 0x2A
                      };
      Data = frameBuilder.BuildLanChecksummed(0x00CC, 0x21, selector);
    }

    public FastClockControlCommand(IZ21FrameBuilder frameBuilder, ModelTime time)
    {
      ArgumentNullException.ThrowIfNull(time);
      byte dayHour = (byte)(((time.Day & 0x07) << 5) | (time.Hour & 0x1F));
      byte minute = (byte)(time.Minute & 0x3F);
      byte rate = (byte)(time.Rate & 0x3F);
      Data = frameBuilder.BuildLanChecksummed(0x00CC, 0x24, 0x2B, dayHour, minute, rate);
    }

    public string Name => "LAN_FAST_CLOCK_CONTROL";

    public byte[] Data { get; }
  }
}
