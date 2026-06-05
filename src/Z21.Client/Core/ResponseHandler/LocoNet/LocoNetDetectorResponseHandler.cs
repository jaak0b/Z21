using System;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.LocoNet
{
  public interface ILocoNetDetectorResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<LocoNetDetectorReceivedEventArgs>? OnLocoNetDetectorReceived;
  }

  /// <summary>
  /// From Z21 FW version 1.22, reports the occupancy status of LocoNet track occupancy detectors
  /// (<c>LAN_LOCONET_DETECTOR</c>, protocol §9.5).
  /// </summary>
  public class LocoNetDetectorResponseHandler : ILocoNetDetectorResponseHandler
  {
    public event EventHandler<LocoNetDetectorReceivedEventArgs>? OnLocoNetDetectorReceived;

    public string Name => "LAN_LOCONET_DETECTOR";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 7 && response[2] == 0xA4 && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      byte type = response[4];
      ushort reportAddress = BitConverter.ToUInt16(response, 5);
      byte[] info = response[7..];
      OnLocoNetDetectorReceived?.Invoke(this, new(type, reportAddress, info));
    }
  }
}
