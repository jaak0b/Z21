using System;
using Z21.Core.Command.SystemState;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.SystemState
{
  public interface IFirmwareVersionResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<FirmwareVersionReceivedEventArgs>? OnFirmwareVersionReceived;
  }

  /// <summary>
  /// Reads the firmware version of the Z21 when requested with <see cref="GetFirmwareVersionCommand"/>.
  /// </summary>
  public class FirmwareVersionResponseHandler : IFirmwareVersionResponseHandler
  {
    public event EventHandler<FirmwareVersionReceivedEventArgs>? OnFirmwareVersionReceived;

    public string Name => "LAN_X_GET_FIRMWARE_VERSION";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 9, (2, 0x40), (3, 0x00), (4, 0xF3), (5, 0x0A))
      && (response[4] ^ response[5] ^ response[6] ^ response[7]) == response[8];

    public void Handle(byte[] response)
    {
      OnFirmwareVersionReceived?.Invoke(this, new(new(BcdToInt(response[6]), BcdToInt(response[7]))));
    }

    protected virtual int BcdToInt(byte bcd)
    {
      int highNibble = (bcd >> 4) & 0xF;
      int lowNibble = bcd & 0xF;
      return highNibble * 10 + lowNibble;
    }
  }
}