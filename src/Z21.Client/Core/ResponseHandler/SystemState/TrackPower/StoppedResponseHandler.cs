using System;
using Z21.Core.Command.SystemState;
using Z21.Core.Command.SystemState.TrackPower;
using Z21.Core.Model;

namespace Z21.Core.ResponseHandler.SystemState.TrackPower
{
  public interface IStoppedResponseHandler : IZ21ResponseHandler
  {
    event EventHandler? OnStoppedReceived;
  }

  /// <summary>
  /// The following packet is sent from the Z21 to the registered clients when a client has sent <see cref="SetStopCommand"/> or the emergency stop was triggered by some input device (multiMaus) and the relevant client has activated the corresponding broadcast the corresponding broadcast <see cref="Z21BroadcastFlags.LocoInfoChangedMessages"/> via <see cref="SetBroadcastFlagsCommand"/>
  /// </summary>
  public class StoppedResponseHandler : IStoppedResponseHandler
  {
    public event EventHandler? OnStoppedReceived;
    
    public string Name => "LAN_X_BC_STOPPED";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 6, (2, 0x40), (3, 0x00), (4, 0x81), (5, 0x00));

    public void Handle(byte[] response)
    {
      OnStoppedReceived?.Invoke(this, EventArgs.Empty);
    }
  }
}