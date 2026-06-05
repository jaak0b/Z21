using System;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Can
{
  public interface ICanDetectorResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<CanDetectorReceivedEventArgs>? OnCanDetectorReceived;
  }

  /// <summary>
  /// From Z21 FW version 1.30, reports a CAN occupancy detector status (<c>LAN_CAN_DETECTOR</c>,
  /// protocol §10.1).
  /// </summary>
  public class CanDetectorResponseHandler : ICanDetectorResponseHandler
  {
    public event EventHandler<CanDetectorReceivedEventArgs>? OnCanDetectorReceived;

    public string Name => "LAN_CAN_DETECTOR";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 14 && response[2] == 0xC4 && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      ushort networkId = BitConverter.ToUInt16(response, 4);
      ushort moduleAddress = BitConverter.ToUInt16(response, 6);
      byte port = response[8];
      byte type = response[9];
      ushort value1 = BitConverter.ToUInt16(response, 10);
      ushort value2 = BitConverter.ToUInt16(response, 12);
      OnCanDetectorReceived?.Invoke(this, new(new CanDetectorData(networkId, moduleAddress, port, type, value1, value2)));
    }
  }
}
