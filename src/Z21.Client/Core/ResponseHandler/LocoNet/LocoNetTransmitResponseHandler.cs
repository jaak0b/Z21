using System;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.LocoNet
{
  public interface ILocoNetTransmitResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<LocoNetMessageReceivedEventArgs>? OnLocoNetMessageReceived;
  }

  /// <summary>
  /// From Z21 FW version 1.20, reports a LocoNet message the Z21 itself wrote onto the bus
  /// (<c>LAN_LOCONET_Z21_TX</c>, protocol §9.2).
  /// </summary>
  public class LocoNetTransmitResponseHandler : ILocoNetTransmitResponseHandler
  {
    public event EventHandler<LocoNetMessageReceivedEventArgs>? OnLocoNetMessageReceived;

    public string Name => "LAN_LOCONET_Z21_TX";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 4 && response[2] == 0xA1 && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      OnLocoNetMessageReceived?.Invoke(this, new(response[4..]));
    }
  }
}
