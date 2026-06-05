using System;
using Z21.Core.Model;

namespace Z21.Core.ResponseParser
{
  public interface IRailComDataParser : IZ21ResponseParser
  {
    RailComData Parse(byte[] data);
  }

  /// <summary>
  /// Parses the RailCom data payload (the bytes following the <c>LAN_RAILCOM_DATACHANGED</c> header).
  /// </summary>
  public class RailComDataParser : IRailComDataParser
  {
    public RailComData Parse(byte[] data)
    {
      ArgumentNullException.ThrowIfNull(data);

      ushort locoAddress = BitConverter.ToUInt16(data, 0);
      uint receiveCounter = BitConverter.ToUInt32(data, 2);
      ushort errorCounter = BitConverter.ToUInt16(data, 6);
      RailComOptions options = (RailComOptions)data[9];
      byte speed = data[10];
      byte qos = data[11];

      return new RailComData(locoAddress, receiveCounter, errorCounter, options, speed, qos);
    }
  }
}
