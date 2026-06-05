using System;

namespace Z21.Core.Framing
{
  public class Z21FrameBuilder : IZ21FrameBuilder
  {
    public byte[] BuildLan(ushort header, params byte[] payload)
    {
      ArgumentNullException.ThrowIfNull(payload);

      ushort length = (ushort)(4 + payload.Length);
      byte[] frame = new byte[length];
      frame[0] = (byte)(length & 0xFF);
      frame[1] = (byte)(length >> 8);
      frame[2] = (byte)(header & 0xFF);
      frame[3] = (byte)(header >> 8);
      Array.Copy(payload, 0, frame, 4, payload.Length);
      return frame;
    }

    public byte[] BuildXBus(byte xHeader, params byte[] data)
    {
      ArgumentNullException.ThrowIfNull(data);

      byte[] xBusPayload = new byte[data.Length + 2];
      xBusPayload[0] = xHeader;
      Array.Copy(data, 0, xBusPayload, 1, data.Length);

      byte xor = xHeader;
      foreach (byte value in data)
        xor ^= value;
      xBusPayload[^1] = xor;

      return BuildLan(0x0040, xBusPayload);
    }

    public byte[] BuildLanChecksummed(ushort header, params byte[] data)
    {
      ArgumentNullException.ThrowIfNull(data);

      byte[] payload = new byte[data.Length + 1];
      Array.Copy(data, 0, payload, 0, data.Length);

      byte xor = 0;
      foreach (byte value in data)
        xor ^= value;
      payload[^1] = xor;

      return BuildLan(header, payload);
    }
  }
}
