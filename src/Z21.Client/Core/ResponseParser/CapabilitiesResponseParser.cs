using Z21.Core.Model;

namespace Z21.Core.ResponseParser
{
  public interface ICapabilitiesResponseParser : IZ21ResponseParser
  {
    Capabilities? Parse(byte statusByte);
  }

  public class CapabilitiesResponseParser : ICapabilitiesResponseParser
  {
    public Capabilities? Parse(byte statusByte)
    {
      if (statusByte == 0)
        return null;
      return new()
             {
               Dcc = (statusByte & 0x01) == 0x01,
               Mm = (statusByte & 0x02) == 0x02,
               RailCom = (statusByte & 0x08) == 0x08,
               LocoCmds = (statusByte & 0x10) == 0x10,
               AccessoryCmds = (statusByte & 0x20) == 0x20,
               DetectorCmds = (statusByte & 0x40) == 0x40,
               NeedsUnlockCode = (statusByte & 0x80) == 0x80,
             };
    }
  }
}