using Z21.Core.Model;

namespace Z21.Core.ResponseParser
{
  public interface ICentralStateResponseParser : IZ21ResponseParser
  {
    public CentralState Parse(byte statusByte);
  }

  public class CentralStateResponseParser : ICentralStateResponseParser
  {
    public CentralState Parse(byte statusByte)
    {
      return new()
             {
               EmergencyStop = (statusByte & 0x01) == 0x01,
               TrackVoltageOff = (statusByte & 0x02) == 0x02,
               ShortCircuit = (statusByte & 0x04) == 0x04,
               ProgrammingModeActive = (statusByte & 0x20) == 0x20
             };
    }
  }
}