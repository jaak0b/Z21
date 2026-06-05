using System;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Feedback
{
  public interface IRmBusDataChangedResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<RmBusDataReceivedEventArgs>? OnRmBusDataReceived;
  }

  /// <summary>
  /// Reports a change on the R-BUS feedback bus (<c>LAN_RMBUS_DATACHANGED</c>, protocol §7.1), either
  /// automatically when the corresponding broadcast is set or in response to <c>LAN_RMBUS_GETDATA</c>.
  /// </summary>
  public class RmBusDataChangedResponseHandler : IRmBusDataChangedResponseHandler
  {
    private const int FeedbackStateCount = 10;

    public event EventHandler<RmBusDataReceivedEventArgs>? OnRmBusDataReceived;

    public string Name => "LAN_RMBUS_DATACHANGED";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 5 + FeedbackStateCount && response[2] == 0x80 && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      byte groupIndex = response[4];
      byte[] feedbackStates = new byte[FeedbackStateCount];
      Buffer.BlockCopy(response, 5, feedbackStates, 0, FeedbackStateCount);
      OnRmBusDataReceived?.Invoke(this, new(groupIndex, feedbackStates));
    }
  }
}
