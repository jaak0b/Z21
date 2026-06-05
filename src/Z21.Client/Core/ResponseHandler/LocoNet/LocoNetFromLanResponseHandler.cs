using System;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.LocoNet
{
  public interface ILocoNetFromLanResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<LocoNetMessageReceivedEventArgs>? OnLocoNetMessageReceived;
  }

  /// <summary>
  /// From Z21 FW version 1.20, reports a LocoNet message another client wrote onto the bus
  /// (<c>LAN_LOCONET_FROM_LAN</c>, protocol §9.3).
  /// </summary>
  public class LocoNetFromLanResponseHandler : ILocoNetFromLanResponseHandler
  {
    public event EventHandler<LocoNetMessageReceivedEventArgs>? OnLocoNetMessageReceived;

    public string Name => "LAN_LOCONET_FROM_LAN";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 4 && response[2] == 0xA2 && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      OnLocoNetMessageReceived?.Invoke(this, new(response[4..]));
    }
  }
}
