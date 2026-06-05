using System;
using Z21.Core.Command.SystemState;
using Z21.Core.Model;

namespace Z21.Core.ResponseHandler.SystemState.TrackPower
{
  public interface ITrackShortResponseHandler : IZ21ResponseHandler
  {
    event EventHandler? OnTrackShortReceived;
  }

  /// <summary>
  /// The following packet is sent from the Z21 to the registered clients if a short circuit has occurred and the relevant client has activated the corresponding broadcast <see cref="Z21BroadcastFlags.LocoInfoChangedMessages"/> via <see cref="SetBroadcastFlagsCommand"/>
  /// </summary>
  public class TrackShortResponseHandler : ITrackShortResponseHandler
  {
    public event EventHandler? OnTrackShortReceived;

    public string Name => "LAN_X_BC_TRACK_SHORT_CIRCUIT";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 6, (2, 0x40), (3, 0x00), (4, 0x61), (5, 0x08));

    public void Handle(byte[] response)
    {
      OnTrackShortReceived?.Invoke(this, EventArgs.Empty);
    }
  }
}