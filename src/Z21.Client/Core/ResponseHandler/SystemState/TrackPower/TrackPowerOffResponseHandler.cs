using System;
using Z21.Core.Command.SystemState;
using Z21.Core.Command.SystemState.TrackPower;
using Z21.Core.Model;

namespace Z21.Core.ResponseHandler.SystemState.TrackPower
{
  public interface ITrackPowerOffResponseHandler : IZ21ResponseHandler
  {
    event EventHandler? OnTrackPowerOffReceived;
  }

  /// <summary>
  /// The following packet is sent from the Z21 to the registered clients when a client has sent command <see cref="SetTrackPowerOffCommand"/>> or the track voltage has been switched off by some input device (multiMaus) and the relevant client has activated the corresponding broadcast <see cref="Z21BroadcastFlags.LocoInfoChangedMessages"/> via <see cref="SetBroadcastFlagsCommand"/>
  /// </summary>
  public class TrackPowerOffResponseHandler : ITrackPowerOffResponseHandler
  {
    public event EventHandler? OnTrackPowerOffReceived;

    public string Name => "LAN_X_BC_TRACK_POWER_OFF";
    
    public bool CanHandle(byte[] response)
    {
      try
      {
        return response[2] == 0x40
               && response[3] == 0x00
               && response[4] == 0x61
               && response[5] == 0x00
               && (response[4] ^ response[5]) == 0x61;
      }
      catch (IndexOutOfRangeException)
      {
        return false;
      }
    }

    public void Handle(byte[] response)
    {
      OnTrackPowerOffReceived?.Invoke(this, EventArgs.Empty);
    }
  }
}