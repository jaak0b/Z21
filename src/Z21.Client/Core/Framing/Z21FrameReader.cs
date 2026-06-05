using System;
using System.Collections.Generic;
using CommandStation.Framing;
using Microsoft.Extensions.Logging;

namespace Z21.Core.Framing
{
  /// <summary>
  /// Splits the Z21 byte stream into frames using the leading little-endian DataLen prefix,
  /// buffering any partial trailing frame until the rest of its bytes arrive.
  /// </summary>
  public class Z21FrameReader : IFrameReader
  {
    /// <summary>
    /// Upper bound for a single frame length. Equals the IPv4-safe UDP payload limit; any declared
    /// <c>DataLen</c> above this is treated as a corrupt prefix and the buffer is resynchronised.
    /// </summary>
    private const int MaxFrameLength = 1472;

    private readonly ILogger<Z21FrameReader>? _logger;
    private readonly List<byte> _buffer = [];

    public Z21FrameReader(ILogger<Z21FrameReader>? logger = null)
    {
      _logger = logger;
    }

    public event EventHandler<FrameReceivedEventArgs>? OnFrameReceived;

    public void Append(byte[] data)
    {
      ArgumentNullException.ThrowIfNull(data);
      _buffer.AddRange(data);

      int offset = 0;
      while (offset + 2 <= _buffer.Count)
      {
        ushort dataLen = (ushort)(_buffer[offset] | (_buffer[offset + 1] << 8));

        if (dataLen == 0 || dataLen > MaxFrameLength)
        {
          _logger?.LogError("Z21FrameReader read an out-of-range frame length {dataLen}; discarding buffered bytes.", dataLen);
          _buffer.Clear();
          return;
        }

        if (offset + dataLen > _buffer.Count)
          break;

        byte[] frame = new byte[dataLen];
        _buffer.CopyTo(offset, frame, 0, dataLen);
        offset += dataLen;
        OnFrameReceived?.Invoke(this, new FrameReceivedEventArgs(frame));
      }

      if (offset > 0)
        _buffer.RemoveRange(0, offset);
    }
  }
}
