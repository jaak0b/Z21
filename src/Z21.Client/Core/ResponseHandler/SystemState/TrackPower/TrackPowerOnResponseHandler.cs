using System;
using Z21.Core.Command.SystemState;
using Z21.Core.Command.SystemState.TrackPower;
using Z21.Core.Model;

namespace Z21.Core.ResponseHandler.SystemState.TrackPower
{
  public interface ITrackPowerOnResponseHandler : IZ21ResponseHandler
  {
    event EventHandler? OnTrackPowerOnReceived;
  }

  /// <summary>
  /// The following packet is sent from the Z21 to the registered clients when a client has sent command <see cref="SetTrackPowerOnCommand"/>> or the track voltage has been switched on by some input device (multiMaus) and the relevant client has activated the corresponding broadcast <see cref="Z21BroadcastFlags.LocoInfoChangedMessages"/> via <see cref="SetBroadcastFlagsCommand"/>
  /// </summary>
  public class TrackPowerOnResponseHandler : ITrackPowerOnResponseHandler
  {
    public event EventHandler? OnTrackPowerOnReceived;
  
    public string Name => "LAN_X_BC_TRACK_POWER_ON";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 6, (2, 0x40), (3, 0x00), (4, 0x61), (5, 0x01));

    public void Handle(byte[] response)
    {
      OnTrackPowerOnReceived?.Invoke(this, EventArgs.Empty);
    }
  }
}