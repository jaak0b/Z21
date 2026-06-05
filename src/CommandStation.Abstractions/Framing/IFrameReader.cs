using System;

namespace CommandStation.Framing
{
  /// <summary>
  /// Reassembles a stream of transport bytes into discrete protocol frames. Implementations buffer
  /// partial frames across calls, so they work over both message-oriented (UDP) and stream-oriented
  /// (serial, TCP) transports.
  /// </summary>
  public interface IFrameReader
  {
    event EventHandler<FrameReceivedEventArgs>? OnFrameReceived;

    /// <summary>
    /// Appends freshly received bytes and raises <see cref="OnFrameReceived"/> for every complete
    /// frame that can now be extracted.
    /// </summary>
    void Append(byte[] data);
  }
}
