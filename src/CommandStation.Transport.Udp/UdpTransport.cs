using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CommandStation.Transport.Udp
{
  public class UdpTransport : ITransport, IDisposable, IAsyncDisposable
  {
    private readonly UdpTransportOptions _options;
    private readonly ILogger<UdpTransport>? _logger;
    private readonly object _sync = new();
    private UdpClient? _udpClient;
    private CancellationTokenSource? _receiveCancellation;

    public UdpTransport(UdpTransportOptions options, ILogger<UdpTransport>? logger = null)
    {
      ArgumentNullException.ThrowIfNull(options);
      _options = options;
      _logger = logger;
    }

    public bool IsConnected { get; private set; }

    public event EventHandler<BytesReceivedEventArgs>? OnBytesReceived;

    public event EventHandler<ConnectionChangedEventArgs>? OnConnectionChanged;

    public Task ConnectAsync()
    {
      UdpClient udpClient;
      CancellationToken token;
      lock (_sync)
      {
        if (IsConnected)
          return Task.CompletedTask;

        int localPort = _options.LocalPort ?? _options.RemoteEndPoint.Port;
        udpClient = new UdpClient(localPort);
        if (OperatingSystem.IsWindows())
          udpClient.AllowNatTraversal(_options.AllowNatTraversal);
        udpClient.Connect(_options.RemoteEndPoint);
        _logger?.LogDebug("[CONN] local endpoint {local} -> remote {remote}", udpClient.Client.LocalEndPoint, _options.RemoteEndPoint);

        _udpClient = udpClient;
        _receiveCancellation = new CancellationTokenSource();
        token = _receiveCancellation.Token;
        IsConnected = true;
      }

      _ = ReceiveLoopAsync(udpClient, token);

      OnConnectionChanged?.Invoke(this, new ConnectionChangedEventArgs(true));
      return Task.CompletedTask;
    }

    public Task DisconnectAsync()
    {
      Disconnect();
      return Task.CompletedTask;
    }

    private void Disconnect()
    {
      CancellationTokenSource? cancellation;
      UdpClient? udpClient;
      lock (_sync)
      {
        if (!IsConnected)
          return;

        IsConnected = false;
        cancellation = _receiveCancellation;
        _receiveCancellation = null;
        udpClient = _udpClient;
        _udpClient = null;
      }

      cancellation?.Cancel();
      cancellation?.Dispose();
      udpClient?.Dispose();

      OnConnectionChanged?.Invoke(this, new ConnectionChangedEventArgs(false));
    }

    public async Task SendAsync(ReadOnlyMemory<byte> data)
    {
      UdpClient udpClient;
      lock (_sync)
      {
        if (_udpClient is null || !IsConnected)
          throw new InvalidOperationException("UdpTransport is not connected.");
        udpClient = _udpClient;
      }

      await udpClient.SendAsync(data);
    }

    public void Dispose()
    {
      Disconnect();
      GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
      Disconnect();
      GC.SuppressFinalize(this);
      return ValueTask.CompletedTask;
    }

    private async Task ReceiveLoopAsync(UdpClient udpClient, CancellationToken cancellationToken)
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        UdpReceiveResult result;
        try
        {
          result = await udpClient.ReceiveAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
          return;
        }
        catch (ObjectDisposedException)
        {
          return;
        }
        catch (SocketException socketException)
        {
          _logger?.LogError(socketException, "UdpTransport receive loop terminated due to a socket error.");
          SignalConnectionLost(udpClient);
          return;
        }

        _logger?.LogDebug("[RX] {length} bytes: {hex}", result.Buffer.Length, BitConverter.ToString(result.Buffer));

        try
        {
          OnBytesReceived?.Invoke(this, new BytesReceivedEventArgs(result.Buffer));
        }
        catch (System.Exception exception)
        {
          _logger?.LogError(exception, "UdpTransport receive loop swallowed an exception thrown by an OnBytesReceived subscriber.");
        }
      }
    }

    private void SignalConnectionLost(UdpClient faultedClient)
    {
      CancellationTokenSource? cancellation = null;
      bool raise = false;
      lock (_sync)
      {
        if (IsConnected && ReferenceEquals(_udpClient, faultedClient))
        {
          IsConnected = false;
          cancellation = _receiveCancellation;
          _receiveCancellation = null;
          _udpClient = null;
          raise = true;
        }
      }

      cancellation?.Dispose();
      faultedClient.Dispose();

      if (raise)
        OnConnectionChanged?.Invoke(this, new ConnectionChangedEventArgs(false));
    }
  }
}
