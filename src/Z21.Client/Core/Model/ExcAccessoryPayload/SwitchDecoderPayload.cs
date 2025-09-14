using System;

namespace Z21.Core.Model.ExcAccessoryPayload
{
  /// <summary>
  /// The 10836 Z21 switch DECODER interprets the payload as "switch decoder with reception of switching time"
  /// </summary>
  public class SwitchDecoderPayload : IExcAccessoryPayload
  {
    /// <summary>
    /// The 10836 Z21 switch DECODER interprets the payload as "switch decoder with reception of switching time"
    /// </summary>
    /// <param name="accessoryOutput">Select the decoder output</param>
    /// <param name="switchTime">A value of 0 means that the output is switched off. A value of 127 means that the output is switched on permanently, i.e. until the next command to this address </param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="switchTime"/> is bigger then 127</exception>
    public SwitchDecoderPayload(AccessoryOutput accessoryOutput, ushort switchTime)
    {
      if (switchTime > 127)
        throw new ArgumentOutOfRangeException(nameof(switchTime), switchTime, "Maximum switch time is 127");

      Payload = (byte)((int)accessoryOutput | switchTime);
    }

    public byte Payload { get; }
  }
}