using System;
using Z21.Core.Command.SystemState;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.SystemState
{
  public interface IHardwareInfoResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<HardwareInfoEventArgs>? OnHardwareInfoReceived;
  }

  /// <summary>
  /// Reads the hardware type and the firmware version of the Z21 when requested via <see cref="GetHardwareInfoCommand"/>.
  /// </summary>
  public class HardwareInfoResponseHandler : IHardwareInfoResponseHandler
  {
    public string Name => "LAN_GET_HWINFO";

    public event EventHandler<HardwareInfoEventArgs>? OnHardwareInfoReceived;

    public bool CanHandle(byte[] response)
    {
      try
      {
        return response[2] == 0x1A && response[3] == 0x00;
      }
      catch (IndexOutOfRangeException)
      {
        return false;
      }
    }

    public void Handle(byte[] response)
    {
      // TODO: calculate FW version and add to HardwareInfoEventArgs
      int hwType = BitConverter.ToInt32(response, 4);
      OnHardwareInfoReceived?.Invoke(this, new(hwType));
    }

  }
}