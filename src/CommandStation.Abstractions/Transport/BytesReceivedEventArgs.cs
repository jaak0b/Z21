using System;

namespace CommandStation.Transport
{
  public class BytesReceivedEventArgs : EventArgs
  {
    public BytesReceivedEventArgs(byte[] data)
    {
      ArgumentNullException.ThrowIfNull(data);
      Data = data;
    }

    public byte[] Data { get; }
  }
}
