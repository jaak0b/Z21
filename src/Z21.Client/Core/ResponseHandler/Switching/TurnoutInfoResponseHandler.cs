using System;
using Microsoft.Extensions.Logging;
using Z21.Core.Codecs;
using Z21.Core.Command.Switching;
using Z21.Core.Command.SystemState;
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
    private readonly IAddressCodec _addressCodec;
    private readonly ILogger<TurnoutInfoResponseHandler>? _logger;

    public TurnoutInfoResponseHandler(IAddressCodec addressCodec, ILogger<TurnoutInfoResponseHandler>? logger = null)
    {
      _addressCodec = addressCodec;
      _logger = logger;
    }

    public event EventHandler<TurnoutInfoReceivedEventArgs>? OnTurnoutInfoReceived;

    public string Name => "LAN_X_TURNOUT_INFO";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 5, (2, 0x40), (3, 0x00), (4, 0x43));

    public void Handle(byte[] response)
    {
      byte msb = response[5];
      byte lsb = response[6];
      ushort address = _addressCodec.CombineAccessoryAddress(lsb, msb);

      byte db2 = response[7];
      AccessoryOutput? accessoryOutput = null;
      if (db2 == 0x1)
        accessoryOutput = AccessoryOutput.Output1;
      if (db2 == 0x02)
        accessoryOutput = AccessoryOutput.Output2;

      _logger?.LogDebug("{name} address {address}, output {accessoryOutput}.", Name, address, accessoryOutput);
      OnTurnoutInfoReceived?.Invoke(this, new(address, accessoryOutput));
    }
  }
}