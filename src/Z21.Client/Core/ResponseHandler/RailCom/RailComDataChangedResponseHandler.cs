using System;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseParser;

namespace Z21.Core.ResponseHandler.RailCom
{
  public interface IRailComDataChangedResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<RailComDataReceivedEventArgs>? OnRailComDataReceived;
  }

  /// <summary>
  /// From Z21 FW version 1.29, reports RailCom data (<c>LAN_RAILCOM_DATACHANGED</c>, protocol §8.1),
  /// either in response to <c>LAN_RAILCOM_GETDATA</c> or unsolicited when the broadcast is active.
  /// </summary>
  public class RailComDataChangedResponseHandler(IRailComDataParser railComDataParser) : IRailComDataChangedResponseHandler
  {
    private const int PayloadLength = 13;

    public event EventHandler<RailComDataReceivedEventArgs>? OnRailComDataReceived;

    public string Name => "LAN_RAILCOM_DATACHANGED";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 4 + PayloadLength && response[2] == 0x88 && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      byte[] data = new byte[PayloadLength];
      Buffer.BlockCopy(response, 4, data, 0, PayloadLength);
      OnRailComDataReceived?.Invoke(this, new(railComDataParser.Parse(data)));
    }
  }
}
