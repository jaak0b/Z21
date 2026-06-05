using System;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.FastClock
{
  public interface IFastClockDataResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<FastClockDataReceivedEventArgs>? OnFastClockDataReceived;
  }

  /// <summary>
  /// From Z21 FW version 1.43, reports the current model time (<c>LAN_FAST_CLOCK_DATA</c>, protocol §12.2).
  /// </summary>
  public class FastClockDataResponseHandler : IFastClockDataResponseHandler
  {
    public event EventHandler<FastClockDataReceivedEventArgs>? OnFastClockDataReceived;

    public string Name => "LAN_FAST_CLOCK_DATA";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 12 && response[2] == 0xCD && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      byte dayHour = response[6];
      byte day = (byte)((dayHour >> 5) & 0x07);
      byte hour = (byte)(dayHour & 0x1F);
      byte minute = (byte)(response[7] & 0x3F);
      byte secondsByte = response[8];
      byte second = (byte)(secondsByte & 0x3F);
      bool isStopped = (secondsByte & 0x80) == 0x80;
      bool isHalted = (secondsByte & 0x40) == 0x40;
      byte rate = (byte)(response[9] & 0x3F);
      FastClockSettings settings = (FastClockSettings)response[10];

      OnFastClockDataReceived?.Invoke(this, new(new FastClockData(day, hour, minute, second, rate, isStopped, isHalted, settings)));
    }
  }
}
