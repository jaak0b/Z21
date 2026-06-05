using System;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.SystemState
{
  public interface IUnknownCommandResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<UnknownCommandReceivedEventArgs>? OnUnknownCommandReceived;
  }

  /// <summary>
  /// The following packet is sent from the Z21 to the client in response to an invalid request.
  /// </summary>
  public class UnknownCommandResponseHandler : IUnknownCommandResponseHandler
  {
    public event EventHandler<UnknownCommandReceivedEventArgs>? OnUnknownCommandReceived;

    public string Name => "LAN_X_UNKNOWN_COMMAND";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 6, (2, 0x40), (3, 0x00), (4, 0x61), (5, 0x82));

    public void Handle(byte[] response)
    {
      OnUnknownCommandReceived?.Invoke(this, new(response));
    }
  }
}