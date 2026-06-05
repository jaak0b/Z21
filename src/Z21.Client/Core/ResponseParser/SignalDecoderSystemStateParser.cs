using System;
using Z21.Core.Model;

namespace Z21.Core.ResponseParser
{
  public interface ISignalDecoderSystemStateParser : IZ21ResponseParser
  {
    SignalDecoderSystemState Parse(byte[] data);
  }

  /// <summary>
  /// Parses the 42-byte signal decoder system state payload (protocol §11.3.4.2).
  /// </summary>
  public class SignalDecoderSystemStateParser : ISignalDecoderSystemStateParser
  {
    public SignalDecoderSystemState Parse(byte[] data)
    {
      ArgumentNullException.ThrowIfNull(data);

      return new SignalDecoderSystemState(
                                          BitConverter.ToInt16(data, 0),
                                          BitConverter.ToInt16(data, 2),
                                          BitConverter.ToUInt16(data, 4),
                                          data[6],
                                          data[7],
                                          data[8..10],
                                          data[10..12],
                                          data[12..16],
                                          data[16..20],
                                          data[23],
                                          data[24..28],
                                          data[28..32],
                                          BitConverter.ToUInt16(data, 32));
    }
  }
}
