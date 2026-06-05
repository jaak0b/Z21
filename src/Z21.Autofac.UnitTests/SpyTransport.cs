using System;
using System.Threading.Tasks;
using CommandStation.Transport;

namespace Z21.Autofac.UnitTests
{
  public class SpyTransport : ITransport
  {
    public bool IsConnected { get; private set; }

    public event EventHandler<BytesReceivedEventArgs>? OnBytesReceived;

    public event EventHandler<ConnectionChangedEventArgs>? OnConnectionChanged;

    public Task ConnectAsync()
    {
      IsConnected = true;
      OnConnectionChanged?.Invoke(this, new ConnectionChangedEventArgs(true));
      return Task.CompletedTask;
    }

    public Task DisconnectAsync()
    {
      IsConnected = false;
      OnConnectionChanged?.Invoke(this, new ConnectionChangedEventArgs(false));
      return Task.CompletedTask;
    }

    public Task SendAsync(ReadOnlyMemory<byte> data) => Task.CompletedTask;

    public void RaiseBytes(byte[] data) => OnBytesReceived?.Invoke(this, new BytesReceivedEventArgs(data));
  }
}
