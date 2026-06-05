using System;
using Z21.Core.Command.SystemState;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseParser;

namespace Z21.Core.ResponseHandler.SystemState
{
  public interface IStatusChangedResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<StatusChangedReceivedEventArgs>? OnStatusChangedReceived;
  }

  /// <summary>
  /// Reads the Z21 status when requested via <see cref="GetStatusCommand"/>.
  /// </summary>
  public class StatusChangedResponseHandler(ICentralStateResponseParser centralStateResponseParser)
    : IStatusChangedResponseHandler
  {
    public event EventHandler<StatusChangedReceivedEventArgs>? OnStatusChangedReceived;

    public string Name => "LAN_X_STATUS_CHANGED";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 8, (2, 0x40), (3, 0x00), (4, 0x62), (5, 0x22))
      && (response[4] ^ response[5] ^ response[6]) == response[7];

    public void Handle(byte[] response)
    {
      byte statusByte = response[6];

      CentralState centralState = centralStateResponseParser.Parse(statusByte);
      OnStatusChangedReceived?.Invoke(this, new(centralState));
    }
  }
}