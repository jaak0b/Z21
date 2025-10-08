using System;
using Z21.Core.Command.SystemState;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.SystemState
{
  public interface IVersionResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<VersionReceivedEventArgs>? OnVersionReceived;
  }

  /// <summary>
  /// Reads the X-Bus version of the Z21 when requested via <see cref="GetVersionCommand"/>.
  /// </summary>
  public class VersionResponseHandler : IVersionResponseHandler
  {
    public event EventHandler<VersionReceivedEventArgs>? OnVersionReceived;

    public string Name => "LAN_X_GET_VERSION";

    public bool CanHandle(byte[] response)
    {
      try
      {
        return response[2] == 0x40 &&
               response[4] == 0x63 &&
               response[5] == 0x21;
        // TODO handle XOR-Byte
      }
      catch (IndexOutOfRangeException)
      {
        return false;
      }
    }

    public void Handle(byte[] response)
    {
      byte xbusVer = response[6];
      byte cmdStationId = response[7];
      OnVersionReceived?.Invoke(this, new(new(xbusVer >> 4, xbusVer & 0x0F), cmdStationId));
    }
  }
}