using System;
using System.Text;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Can
{
  public interface ICanDeviceDescriptionResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<CanDeviceDescriptionReceivedEventArgs>? OnCanDeviceDescriptionReceived;
  }

  /// <summary>
  /// From Z21 FW version 1.41, reports the description of a CAN booster
  /// (<c>LAN_CAN_DEVICE_GET_DESCRIPTION</c> reply, protocol §10.2.1).
  /// </summary>
  public class CanDeviceDescriptionResponseHandler : ICanDeviceDescriptionResponseHandler
  {
    private const int NameLength = 16;

    public event EventHandler<CanDeviceDescriptionReceivedEventArgs>? OnCanDeviceDescriptionReceived;

    public string Name => "LAN_CAN_DEVICE_GET_DESCRIPTION";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 6 + NameLength && response[2] == 0xC8 && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      ushort networkId = BitConverter.ToUInt16(response, 4);
      string name = Encoding.Latin1.GetString(response, 6, NameLength);
      int terminator = name.IndexOf('\0');
      if (terminator >= 0)
        name = name[..terminator];
      OnCanDeviceDescriptionReceived?.Invoke(this, new(networkId, name));
    }
  }
}
