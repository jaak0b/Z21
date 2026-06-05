using System;

namespace CommandStation.Framing
{
  public class FrameReceivedEventArgs : EventArgs
  {
    public FrameReceivedEventArgs(byte[] frame)
    {
      ArgumentNullException.ThrowIfNull(frame);
      Frame = frame;
    }

    public byte[] Frame { get; }
  }
}
