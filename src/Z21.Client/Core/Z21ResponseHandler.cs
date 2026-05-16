using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Transport;

namespace Z21.Core
{
  public class Z21ResponseHandler
  {
    private readonly IZ21Transport _transport;
    private readonly IEnumerable<IZ21ResponseHandler> _z21ResponseHandlers;
    private readonly ILogger<Z21ResponseHandler>? _logger;

    public Z21ResponseHandler(IZ21Transport z21Transport, IEnumerable<IZ21ResponseHandler> z21ResponseHandlers, ILogger<Z21ResponseHandler>? logger = null)
    {
      _transport = z21Transport;
      _z21ResponseHandlers = z21ResponseHandlers;
      _logger = logger;
      _transport.OnResponseReceived += Transport_OnResponseReceived;
    }

    protected virtual void Transport_OnResponseReceived(object? sender, ResponseReceivedEventArgs bytes)
    {
      CutDatagram(bytes.Response).ForEach(HandleDatagram);
    }

    protected virtual void HandleDatagram(byte[] data)
    {
      foreach (IZ21ResponseHandler handler in _z21ResponseHandlers.Where(handler => handler.CanHandle(data)))
      {
        try
        {
          _logger?.LogDebug("{handlerName} handling datagram {cutDatagram}.", handler.Name, BitConverter.ToString(data));
          handler.Handle(data);
        }
        catch (System.Exception exception)
        {
          _logger?.LogError(exception, "{handlerName} failed to handle datagram {cutDatagram}.", handler.Name, BitConverter.ToString(data));
        }
      }
    }

    protected virtual List<byte[]> CutDatagram(byte[] datagram)
    {
      List<byte[]> cutDatagrams = [];
      int offset = 0;
      while (offset < datagram.Length)
      {
        try
        {
          if (offset + 2 > datagram.Length)
          {
            _logger?.LogError("Incomplete DataLen field — discarding remainder. Data: {datagram}", BitConverter.ToString(datagram));
            return cutDatagrams;
          }

          ushort dataLen = (ushort)(datagram[offset] | (datagram[offset + 1] << 8));

          if (offset + dataLen > datagram.Length)
          {
            _logger?.LogError("Incomplete packet — discarding remainder. Data: {datagram}", BitConverter.ToString(datagram));
            return cutDatagrams;
          }

          byte[] cutDatagram = new byte[dataLen];
          Buffer.BlockCopy(datagram, offset, cutDatagram, 0, dataLen);
          _logger?.LogDebug("Received cut datagram: {cutDatagram}", BitConverter.ToString(cutDatagram));
          offset += dataLen;
          cutDatagrams.Add(cutDatagram);
        }
        catch (System.Exception exception)
        {
          _logger?.LogError(exception, "Failed to cut datagram — discarding remainder. Data: {datagram}", BitConverter.ToString(datagram));
          return cutDatagrams;
        }
      }

      return cutDatagrams;
    }
  }
}