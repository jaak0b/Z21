using System;
using System.Text;
using Z21.Core.Model;

namespace Z21.Core.ResponseParser
{
  public interface IZLinkHardwareInfoParser : IZ21ResponseParser
  {
    ZLinkHardwareInfo Parse(byte[] data);
  }

  /// <summary>
  /// Parses the 58-byte <c>Z_Hw_Info</c> payload of a Z21 pro LINK (protocol §11.1.1.1).
  /// </summary>
  public class ZLinkHardwareInfoParser : IZLinkHardwareInfoParser
  {
    public ZLinkHardwareInfo Parse(byte[] data)
    {
      ArgumentNullException.ThrowIfNull(data);

      ushort hardwareId = BitConverter.ToUInt16(data, 0);
      byte major = data[2];
      byte minor = data[3];
      ushort build = BitConverter.ToUInt16(data, 4);
      string mac = ReadString(data, 6, 18);
      string name = ReadString(data, 24, 33);

      return new ZLinkHardwareInfo(hardwareId, major, minor, build, mac, name);
    }

    private string ReadString(byte[] data, int offset, int length)
    {
      string value = Encoding.Latin1.GetString(data, offset, length);
      int terminator = value.IndexOf('\0');
      return terminator >= 0 ? value[..terminator] : value;
    }
  }
}
