using System;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.LocoNet
{
  public interface ILocoNetDispatchAddressResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<LocoNetDispatchAddressReceivedEventArgs>? OnLocoNetDispatchAddressReceived;
  }

  /// <summary>
  /// From Z21 FW version 1.22, reports the result of a LocoNet dispatch request
  /// (<c>LAN_LOCONET_DISPATCH_ADDR</c>, protocol §9.4).
  /// </summary>
  public class LocoNetDispatchAddressResponseHandler : ILocoNetDispatchAddressResponseHandler
  {
    public event EventHandler<LocoNetDispatchAddressReceivedEventArgs>? OnLocoNetDispatchAddressReceived;

    public string Name => "LAN_LOCONET_DISPATCH_ADDR";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 7 && response[2] == 0xA3 && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      ushort locoAddress = BitConverter.ToUInt16(response, 4);
      byte slot = response[6];
      OnLocoNetDispatchAddressReceived?.Invoke(this, new(locoAddress, slot));
    }
  }
}
