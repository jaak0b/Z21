using System;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseParser;

namespace Z21.Core.ResponseHandler.Decoder
{
  public interface IDecoderSystemStateResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<SwitchDecoderSystemStateReceivedEventArgs>? OnSwitchDecoderSystemStateReceived;

    event EventHandler<SignalDecoderSystemStateReceivedEventArgs>? OnSignalDecoderSystemStateReceived;
  }

  /// <summary>
  /// Reports a zLink decoder system state (<c>LAN_DECODER_SYSTEMSTATE_DATACHANGED</c>, protocol §11.3.4).
  /// The switch decoder (10836) and signal decoder (10837) layouts are distinguished by the frame length.
  /// </summary>
  public class DecoderSystemStateResponseHandler(ISwitchDecoderSystemStateParser switchParser, ISignalDecoderSystemStateParser signalParser) : IDecoderSystemStateResponseHandler
  {
    private const int SwitchFrameLength = 48;
    private const int SignalFrameLength = 46;

    public event EventHandler<SwitchDecoderSystemStateReceivedEventArgs>? OnSwitchDecoderSystemStateReceived;

    public event EventHandler<SignalDecoderSystemStateReceivedEventArgs>? OnSignalDecoderSystemStateReceived;

    public string Name => "LAN_DECODER_SYSTEMSTATE_DATACHANGED";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return (response.Length == SwitchFrameLength || response.Length == SignalFrameLength) && response[2] == 0xDA && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      byte[] payload = response[4..];
      if (response.Length == SwitchFrameLength)
        OnSwitchDecoderSystemStateReceived?.Invoke(this, new(switchParser.Parse(payload)));
      else
        OnSignalDecoderSystemStateReceived?.Invoke(this, new(signalParser.Parse(payload)));
    }
  }
}
