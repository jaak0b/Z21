using System;
using System.Collections.Generic;
using CommandStation.Framing;
using CommandStation.Transport;
using Microsoft.Extensions.Logging;
using Z21.Core.ResponseHandler;

namespace Z21.Core
{
  public class Z21ResponseHandler
  {
    private readonly ITransport _transport;
    private readonly IFrameReader _frameReader;
    private readonly IEnumerable<IZ21ResponseHandler> _z21ResponseHandlers;
    private readonly ILogger<Z21ResponseHandler>? _logger;

    public Z21ResponseHandler(ITransport transport, IFrameReader frameReader, IEnumerable<IZ21ResponseHandler> z21ResponseHandlers, ILogger<Z21ResponseHandler>? logger = null)
    {
      _transport = transport;
      _frameReader = frameReader;
      _z21ResponseHandlers = z21ResponseHandlers;
      _logger = logger;
      _frameReader.OnFrameReceived += FrameReader_OnFrameReceived;
      _transport.OnBytesReceived += Transport_OnBytesReceived;
    }

    protected virtual void Transport_OnBytesReceived(object? sender, BytesReceivedEventArgs args)
    {
      _frameReader.Append(args.Data);
    }

    protected virtual void FrameReader_OnFrameReceived(object? sender, FrameReceivedEventArgs args)
    {
      HandleDatagram(args.Frame);
    }

    protected virtual void HandleDatagram(byte[] data)
    {
      foreach (IZ21ResponseHandler handler in _z21ResponseHandlers)
      {
        try
        {
          if (!handler.CanHandle(data))
            continue;

          _logger?.LogDebug("{handlerName} handling datagram {cutDatagram}.", handler.Name, BitConverter.ToString(data));
          handler.Handle(data);
        }
        catch (System.Exception exception)
        {
          _logger?.LogError(exception, "{handlerName} failed to handle datagram {cutDatagram}.", handler.Name, BitConverter.ToString(data));
        }
      }
    }
  }
}
