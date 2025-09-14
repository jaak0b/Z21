using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;
using Z21.Core.Command;
using Z21.Core.Command.SystemState;
using Z21.Core.Exception;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Transport;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Z21.Core
{
  public class Z21Client : IZ21Client
  {
    private bool _previousIsConnected;
    private DateTime _lastCommunication = DateTime.MinValue;
    private readonly Timer _connectionKeepAlive;
    private readonly ILogger<Z21Client> _logger;
    private readonly Z21Configuration _z21Configuration;
    private readonly IZ21Transport _transport;
    private readonly List<IZ21ResponseHandler> _handlers;

    /// <summary>
    /// IPv4 safe MTU for payload according to specification.
    /// </summary>
    public const int MaxUdpPayload = 1472;

    /// <exception cref="PlatformNotSupportedException">Thrown when system architecture is not little-endian.</exception>
    public Z21Client(Z21Configuration z21Configuration, IZ21Transport z21Transport, IEnumerable<IZ21ResponseHandler> z21ResponseHandlers, ILogger<Z21Client> logger)
    {
      ArgumentNullException.ThrowIfNull(z21Configuration, nameof(z21Configuration));
      ArgumentNullException.ThrowIfNull(z21Transport, nameof(z21Transport));
      ArgumentNullException.ThrowIfNull(z21ResponseHandlers, nameof(z21ResponseHandlers));
      ArgumentNullException.ThrowIfNull(logger, nameof(logger));

      if (!BitConverter.IsLittleEndian)
        throw new PlatformNotSupportedException("Z21Client requires little-endian architecture.");

      _z21Configuration = z21Configuration;
      _transport = z21Transport;
      _handlers = z21ResponseHandlers.ToList();
      _logger = logger;
      _transport.OnResponseReceived += Transport_OnResponseReceived;

      _connectionKeepAlive = new(z21Configuration.ConnectionKeepAliveCommandInterval)
                             {
                               AutoReset = true,
                               Enabled = false
                             };
      _connectionKeepAlive.Elapsed += ConnectionKeepAlive_OnElapsed;
    }

    public event EventHandler<ConnectionChangedEventArgs>? OnConnectionChanged;

    public bool IsConnected => DateTime.UtcNow - _lastCommunication < _z21Configuration.ConnectionKeepAliveCommandInterval + _z21Configuration.ResponseTime;

    public async Task ConnectAsync()
    {
      _logger.LogInformation("Z21Client trying to connect with {ClientIPEndPoint}.", _transport.Z21Configuration.ClientIPEndPoint);
      _transport.Connect();
      _connectionKeepAlive.Enabled = true;
      await SendCommandsAsync(new GetFirmwareVersionCommand());
    }

    public async Task SendCommandsAsync(params IZ21Command[] z21Commands)
    {
      ArgumentNullException.ThrowIfNull(z21Commands, nameof(z21Commands));

      if (!_transport.IsConnected)
        throw new ClientNotConnectedException();

      foreach (IZ21Command z21Command in z21Commands)
        _logger.LogDebug("{commandName} sending {datagram} to Z21.", z21Command.Name, BitConverter.ToString(z21Command.Data));

      byte[] combinedPayload = z21Commands.SelectMany(z21Command => z21Command.Data).ToArray();

      MtuPayloadLengthExceededException.ThrowIfExceeded(combinedPayload);

      _connectionKeepAlive.Stop();
      _connectionKeepAlive.Start();
      await VerifyConnectionOnDemandAsync();

      await _transport.SendAsync(combinedPayload);
    }

    public List<byte[]> CutDatagram(byte[] datagram)
    {
      List<byte[]> cutDatagrams = [];
      int offset = 0;
      while (offset < datagram.Length)
      {
        try
        {
          if (offset + 2 > datagram.Length)
          {
            _logger.LogError("Incomplete DataLen field — discarding remainder. Data: {datagram}", BitConverter.ToString(datagram));
            return cutDatagrams;
          }

          ushort dataLen = (ushort)(datagram[offset] | (datagram[offset + 1] << 8));

          if (offset + dataLen > datagram.Length)
          {
            _logger.LogError("Incomplete packet — discarding remainder. Data: {datagram}", BitConverter.ToString(datagram));
            return cutDatagrams;
          }

          byte[] cutDatagram = new byte[dataLen];
          Buffer.BlockCopy(datagram, offset, cutDatagram, 0, dataLen);
          _logger.LogDebug("Received cut datagram: {cutDatagram}", BitConverter.ToString(cutDatagram));
          offset += dataLen;
          cutDatagrams.Add(cutDatagram);
        }
        catch (System.Exception exception)
        {
          _logger.LogError(exception, "Failed to cut datagram — discarding remainder. Data: {datagram}", BitConverter.ToString(datagram));
          return cutDatagrams;
        }
      }

      return cutDatagrams;
    }

    public void HandleDatagram(byte[] data)
    {
      foreach (IZ21ResponseHandler handler in _handlers.Where(handler => handler.CanHandle(data)))
      {
        try
        {
          _logger.LogDebug("{handlerName} handling datagram {cutDatagram}.", handler.Name, BitConverter.ToString(data));
          handler.Handle(data);
        }
        catch (System.Exception exception)
        {
          _logger.LogError(exception, "{handlerName} failed to handle datagram {cutDatagram}.", handler.Name, BitConverter.ToString(data));
        }
      }
    }

    private async Task VerifyConnectionOnDemandAsync()
    {
      bool previousIsConnected = _previousIsConnected;
      _previousIsConnected = IsConnected;

      if (previousIsConnected != IsConnected)
      {
        if (IsConnected)
        {
          _logger.LogInformation("Z21Client connecting with {ClientIPEndPoint}.", _transport.Z21Configuration.ClientIPEndPoint);
          await LogOnAsync();
        }
        else
        {
          _logger.LogInformation("Z21Client lost connection with {ClientIPEndPoint}.", _transport.Z21Configuration.ClientIPEndPoint);
        }
        OnConnectionChanged?.Invoke(this, new(IsConnected));
      }
    }

    protected async virtual Task LogOnAsync()
    {
      await SendCommandsAsync(new SetBroadcastFlagsCommand(_z21Configuration.BroadcastFlags), new GetFirmwareVersionCommand());
    }

    private async void Transport_OnResponseReceived(object? sender, ResponseReceivedEventArgs bytes)
    {
      _lastCommunication = DateTime.UtcNow;
      await VerifyConnectionOnDemandAsync();
      
      CutDatagram(bytes.Response).ForEach(HandleDatagram);
    }

    private async void ConnectionKeepAlive_OnElapsed(object? sender, ElapsedEventArgs e)
    {
      await SendCommandsAsync(new GetFirmwareVersionCommand());
    }
  }
}