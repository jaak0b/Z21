using System;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.FastClock
{
  public interface IFastClockSettingsResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<FastClockSettingsReceivedEventArgs>? OnFastClockSettingsReceived;
  }

  /// <summary>
  /// Reports the persistent fast-clock settings (<c>LAN_FAST_CLOCK_SETTINGS_GET</c> reply, protocol §12.3).
  /// </summary>
  public class FastClockSettingsResponseHandler : IFastClockSettingsResponseHandler
  {
    public event EventHandler<FastClockSettingsReceivedEventArgs>? OnFastClockSettingsReceived;

    public string Name => "LAN_FAST_CLOCK_SETTINGS_GET";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 8 && response[2] == 0xCE && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      FastClockSettings settings = (FastClockSettings)response[4];
      byte rate = response[5];
      byte startDayHour = response[6];
      byte startMinute = response[7];
      OnFastClockSettingsReceived?.Invoke(this, new(new FastClockSettingsData(settings, rate, startDayHour, startMinute)));
    }
  }
}
