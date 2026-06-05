using System;
using System.Buffers.Binary;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Settings
{
  public interface IAccessoryModeResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<DecoderModeReceivedEventArgs>? OnAccessoryModeReceived;
  }

  /// <summary>
  /// Read the settings for a given accessory decoder address ("Accessory Decoder" RP-9.2.1).
  /// </summary>
  /// <remarks>
  /// In the Z21, the output format (DCC, MM) is persistently stored for each accessory decoder address. A maximum of 256 different accessory decoder addresses can be stored. Each address >= 256 automatically is DCC.
  /// </remarks>
  public class AccessoryModeResponseHandler : IAccessoryModeResponseHandler
  {

    public event EventHandler<DecoderModeReceivedEventArgs>? OnAccessoryModeReceived;

    public string Name => "LAN_GET_TURNOUTMODE";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 4, (2, 0x70), (3, 0x00));

    public void Handle(byte[] response)
    {
      byte[] cutDatagram = new byte[2];
      Buffer.BlockCopy(response, 4, cutDatagram, 0, 2);
      short locoAddress = BinaryPrimitives.ReadInt16BigEndian(cutDatagram);

      byte rawMode = response[6];
      DecoderMode mode = Enum.IsDefined(typeof(DecoderMode), (int)rawMode) ? (DecoderMode)rawMode : DecoderMode.Unknown;

      OnAccessoryModeReceived?.Invoke(this, new(locoAddress, mode));
    }
  }
}