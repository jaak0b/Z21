using Z21.Core.Model;

namespace Z21.Core.ResponseParser
{
  public interface ICentralStateExResponseParser : IZ21ResponseParser
  {
    CentralStateEx Parse(byte statusByte);
  }

  public class CentralStateExResponseParser : ICentralStateExResponseParser
  {
    public CentralStateEx Parse(byte statusByte)
    {
      return new()
             {
               HighTemperature = (statusByte & 0x01) == 0x01,
               PowerLost = (statusByte & 0x02) == 0x02,
               ShortCircuitExternal = (statusByte & 0x04) == 0x04,
               ShortCircuitInternal = (statusByte & 0x08) == 0x08,
               Rcn213 = (statusByte & 0x20) == 0x20,
             };
    }
  }
}