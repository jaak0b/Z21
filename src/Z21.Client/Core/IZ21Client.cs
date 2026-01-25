using System;
using System.Threading.Tasks;
using Z21.Core.Command;
using Z21.Core.Exception;
using Z21.Core.Model.EventArgs;

namespace Z21.Core
{
  public interface IZ21Client
  {
    event EventHandler<ConnectionChangedEventArgs>? OnConnectionChanged;
    
    /// <summary>
    /// Sends <paramref name="z21Commands"/> to the Z21.
    /// </summary>
    /// <exception cref="MtuPayloadLengthExceededException">Thrown if command length exceeds max udp payload length.</exception>
    /// <exception cref="ClientNotConnectedException">Thrown when <see cref="ConnectAsync"/> has not yet been called.</exception>
    Task SendCommandsAsync(params IZ21Command[] z21Commands);

    Task ConnectAsync();
    
    bool IsConnected { get; }
  }
}