using System;
using Z21.Core.Model;

namespace Z21.Core.ResponseParser
{
  public interface ISwitchDecoderSystemStateParser : IZ21ResponseParser
  {
    SwitchDecoderSystemState Parse(byte[] data);
  }

  /// <summary>
  /// Parses the 44-byte switch decoder system state payload (protocol §11.3.4.1).
  /// </summary>
  public class SwitchDecoderSystemStateParser : ISwitchDecoderSystemStateParser
  {
    public SwitchDecoderSystemState Parse(byte[] data)
    {
      ArgumentNullException.ThrowIfNull(data);

      return new SwitchDecoderSystemState(
                                          BitConverter.ToInt16(data, 0),
                                          BitConverter.ToInt16(data, 2),
                                          BitConverter.ToUInt16(data, 4),
                                          data[6],
                                          data[7],
                                          data[8..16],
                                          data[16..24],
                                          data[24..32],
                                          BitConverter.ToUInt16(data, 32),
                                          BitConverter.ToUInt16(data, 34),
                                          data[42]);
    }
  }
}
