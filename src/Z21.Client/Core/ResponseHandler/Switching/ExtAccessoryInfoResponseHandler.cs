using System;
using Z21.Core.Codecs;
using Z21.Core.Command.Switching;
using Z21.Core.Command.SystemState;
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
    private readonly IAddressCodec _addressCodec;

    public ExtAccessoryInfoResponseHandler(IAddressCodec addressCodec)
    {
      _addressCodec = addressCodec;
    }

    public event EventHandler<ExtAccessoryInfoReceivedEventArgs>? OnExtAccessoryInfoReceived;

    public string Name => "LAN_X_EXT_ACCESSORY_INFO";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 5, (2, 0x40), (3, 0x00), (4, 0x44));

    public void Handle(byte[] response)
    {
      byte msb = response[5];
      byte lsb = response[6];
      ushort address = _addressCodec.CombineExtAccessoryAddress(lsb, msb);

      byte db2 = response[7];
      byte status = response[8];

      OnExtAccessoryInfoReceived?.Invoke(this, new(address, db2, status == 0));
    }
  }
}