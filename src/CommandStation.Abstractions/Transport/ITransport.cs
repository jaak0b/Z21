using System;
using System.Threading.Tasks;

namespace CommandStation.Transport
{
  /// <summary>
  /// A protocol-agnostic byte pipe to a command station. Implementations may use any medium
  /// (UDP, TCP, serial, …) and deliver received bytes in arbitrary chunk sizes.
  /// </summary>
  public interface ITransport
  {
    bool IsConnected { get; }

    event EventHandler<BytesReceivedEventArgs>? OnBytesReceived;

    event EventHandler<ConnectionChangedEventArgs>? OnConnectionChanged;

    /// <summary>
    /// Opens the underlying connection and begins receiving.
    /// </summary>
    Task ConnectAsync();

    /// <summary>
    /// Closes the underlying connection.
    /// </summary>
    Task DisconnectAsync();

    /// <summary>
    /// Sends the given bytes to the command station.
    /// </summary>
    Task SendAsync(ReadOnlyMemory<byte> data);
  }
}
