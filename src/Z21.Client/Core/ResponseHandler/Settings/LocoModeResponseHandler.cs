using System;
using System.Buffers.Binary;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Settings
{
  public interface ILocoModeResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<DecoderModeReceivedEventArgs>? OnLocoModeReceived;
  }

  /// <summary>
  /// Read the output format for a given locomotive address.
  /// </summary>
  /// <remarks>
  /// In the Z21, the output format (DCC, MM) is persistently stored for each locomotive address. A maximum of 256 different locomotive addresses can be stored. Each address >= 256 is DCC automatically.
  /// </remarks>
  public class LocoModeResponseHandler : ILocoModeResponseHandler
  {
    public event EventHandler<DecoderModeReceivedEventArgs>? OnLocoModeReceived;

    public string Name => "LAN_GET_LOCOMODE";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 4, (2, 0x60), (3, 0x00));

    public void Handle(byte[] response)
    {
      byte[] cutDatagram = new byte[2];
      Buffer.BlockCopy(response, 4, cutDatagram, 0, 2);
      short locoAddress = BinaryPrimitives.ReadInt16BigEndian(cutDatagram);

      byte rawMode = response[6];
      DecoderMode mode = Enum.IsDefined(typeof(DecoderMode), (int)rawMode) ? (DecoderMode)rawMode : DecoderMode.Unknown;

      OnLocoModeReceived?.Invoke(this, new(locoAddress, mode));
    }
  }
}