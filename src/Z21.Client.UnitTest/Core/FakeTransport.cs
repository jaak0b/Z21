using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandStation.Transport;

namespace Z21.UnitTest.Core
{
  public class FakeTransport : ITransport
  {
    public List<byte[]> Sent { get; } = [];

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

    public Task SendAsync(ReadOnlyMemory<byte> data)
    {
      Sent.Add(data.ToArray());
      return Task.CompletedTask;
    }

    public void RaiseBytes(byte[] data) => OnBytesReceived?.Invoke(this, new BytesReceivedEventArgs(data));

    /// <summary>Simulates a transport-level connection loss (e.g. socket error), independent of <see cref="DisconnectAsync"/>.</summary>
    public void RaiseConnectionLost()
    {
      IsConnected = false;
      OnConnectionChanged?.Invoke(this, new ConnectionChangedEventArgs(false));
    }

    /// <summary>Forces the connected flag without raising events, to model the transport reconnecting on its own.</summary>
    public void SetConnected(bool value) => IsConnected = value;
  }
}
