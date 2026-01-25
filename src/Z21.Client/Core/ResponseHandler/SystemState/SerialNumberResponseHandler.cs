using System;
using Z21.Core.Command.SystemState;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.SystemState
{
  public interface ISerialNumberResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<SerialNumberReceivedEventArgs>? OnSerialNumberReceived;
  }

  /// <summary>
  /// Reads the serial number of the Z21 when requested via <see cref="GetSerialNumberCommand"/>.
  /// </summary>
  public class SerialNumberResponseHandler : ISerialNumberResponseHandler
  {
    public event EventHandler<SerialNumberReceivedEventArgs>? OnSerialNumberReceived;

    public string Name => "LAN_GET_SERIAL_NUMBER";

    public bool CanHandle(byte[] response)
    {
      try
      {
        return response[2] == 0x10
               && response[3] == 0x00;
      }
      catch (IndexOutOfRangeException)
      {
        return false;
      }
    }

    public void Handle(byte[] response)
    {
      uint serialNumber = BitConverter.ToUInt32(response, 4);
      OnSerialNumberReceived?.Invoke(this, new(serialNumber));
    }
  }
}