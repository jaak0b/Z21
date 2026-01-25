using System;
using Z21.Core.Command.Switching;
using Z21.Core.Command.SystemState;
using Z21.Core.Helper;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Switching
{
  public interface ITurnoutInfoResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<TurnoutInfoReceivedEventArgs>? OnTurnoutInfoReceived;
  }

  /// <summary>
  /// This message is sent from the Z21 to the clients in response to the command <see cref="GetTurnoutInfoCommand"/>.
  /// <para/> It is also sent to an associated client unsolicitedly if the function status has been changed by one of the (other) clients or a handset controller and the associated client has activated the corresponding broadcast <see cref="Z21BroadcastFlags.LocoInfoChangedMessages"/> via <see cref="SetBroadcastFlagsCommand"/>
  /// </summary>
  public class TurnoutInfoResponseHandler : ITurnoutInfoResponseHandler
  {

    public event EventHandler<TurnoutInfoReceivedEventArgs>? OnTurnoutInfoReceived;

    public string Name => "LAN_X_TURNOUT_INFO";

    public bool CanHandle(byte[] response)
    {
      try
      {
        return response[2] == 0x40 && response[3] == 0x00 && response[4] == 0x43;
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
      AccessoryOutput? accessoryOutput = null;
      if (db2 == 0x1)
        accessoryOutput = AccessoryOutput.Output1;
      if (db2 == 0x02)
        accessoryOutput = AccessoryOutput.Output2;

      Console.WriteLine($"Turnout: {address}, State: {accessoryOutput}");
      OnTurnoutInfoReceived?.Invoke(this, new(address, accessoryOutput));
    }
  }
}