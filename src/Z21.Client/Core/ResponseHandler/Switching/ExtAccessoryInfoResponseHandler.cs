using System;
using Z21.Core.Command.Switching;
using Z21.Core.Command.SystemState;
using Z21.Core.Helper;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Switching
{
  public interface IExtAccessoryInfoResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<ExtAccessoryInfoReceivedEventArgs>? OnExtAccessoryInfoReceived;
  }

  /// <summary>
  /// Reads the encoded extended accessory decoder state either because it was requested via <see cref="GetExtAccessoryInfoCommand"/> or the accessory status has been changed by other clients or a handset controller and the client has activated the corresponding broadcast <see cref="Z21BroadcastFlags.DriveAndSwitchingMessages"/> via <see cref="SetBroadcastFlagsCommand"/> .
  /// </summary>
  public class ExtAccessoryInfoResponseHandler : IExtAccessoryInfoResponseHandler
  {
    public event EventHandler<ExtAccessoryInfoReceivedEventArgs>? OnExtAccessoryInfoReceived;

    public string Name => "LAN_X_EXT_ACCESSORY_INFO";

    public bool CanHandle(byte[] response)
    {
      try
      {
        return response[2] == 0x40 && response[3] == 0x00 && response[4] == 0x44;
      }
      catch (IndexOutOfRangeException)
      {
        return false;
      }
    }

    public void Handle(byte[] response)
    {
      byte msb = response[5];
      byte lsb = response[6];
      ushort address = AddressHelper.CombineAccessoryAddress(lsb, msb);

      byte db2 = response[7];
      byte status = response[8];

      OnExtAccessoryInfoReceived?.Invoke(this, new(address, db2, status == 0));
    }
  }
}