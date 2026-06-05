using System;
using System.Text;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Decoder
{
  public interface IDecoderDescriptionResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<DecoderDescriptionReceivedEventArgs>? OnDecoderDescriptionReceived;
  }

  /// <summary>
  /// Reports the description of a zLink decoder (<c>LAN_DECODER_GET_DESCRIPTION</c> reply, protocol §11.3.1).
  /// </summary>
  public class DecoderDescriptionResponseHandler : IDecoderDescriptionResponseHandler
  {
    private const int NameLength = 32;

    public event EventHandler<DecoderDescriptionReceivedEventArgs>? OnDecoderDescriptionReceived;

    public string Name => "LAN_DECODER_GET_DESCRIPTION";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 4 + NameLength && response[2] == 0xD8 && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      string name = Encoding.Latin1.GetString(response, 4, NameLength);
      int terminator = name.IndexOf('\0');
      if (terminator >= 0)
        name = name[..terminator];
      OnDecoderDescriptionReceived?.Invoke(this, new(name));
    }
  }
}
