using System;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseParser;

namespace Z21.Core.ResponseHandler.ZLink
{
  public interface IZLinkHardwareInfoResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<ZLinkHardwareInfoReceivedEventArgs>? OnZLinkHardwareInfoReceived;
  }

  /// <summary>
  /// Reports the hardware information of a Z21 pro LINK (<c>LAN_ZLINK_GET_HWINFO</c> reply, protocol §11.1.1.1).
  /// </summary>
  public class ZLinkHardwareInfoResponseHandler(IZLinkHardwareInfoParser parser) : IZLinkHardwareInfoResponseHandler
  {
    private const int FrameLength = 63;

    public event EventHandler<ZLinkHardwareInfoReceivedEventArgs>? OnZLinkHardwareInfoReceived;

    public string Name => "LAN_ZLINK_GET_HWINFO";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= FrameLength && response[2] == 0xE8 && response[3] == 0x00 && response[4] == 0x06;
    }

    public void Handle(byte[] response)
    {
      OnZLinkHardwareInfoReceived?.Invoke(this, new(parser.Parse(response[5..])));
    }
  }
}
