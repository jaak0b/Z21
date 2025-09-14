using System;
using Z21.Core.Model;

namespace Z21.Core.ResponseParser
{
  public interface ISystemStateResponseParser : IZ21ResponseParser
  {
    public SystemState Parse(byte[] data);
  }

  public class SystemStateResponseParser(ICentralStateResponseParser centralStateResponseParser, ICentralStateExResponseParser centralStateExResponseParser, ICapabilitiesResponseParser capabilitiesResponseParser)
    : ISystemStateResponseParser
  {
    public SystemState Parse(byte[] data)
    {
      return new()
             {
               MainCurrent = BitConverter.ToInt16(data, 0),
               ProgCurrent = BitConverter.ToInt16(data, 2),
               FilteredMainCurrent = BitConverter.ToInt16(data, 4),
               Temperature = BitConverter.ToInt16(data, 6),
               SupplyVoltage = BitConverter.ToUInt16(data, 8),
               VccVoltage = BitConverter.ToUInt16(data, 10),
               CentralState = centralStateResponseParser.Parse(data[12]),
               CentralStateEx = centralStateExResponseParser.Parse(data[13]),
               Capabilities = capabilitiesResponseParser.Parse(data[15])
             };
    }
  }
}