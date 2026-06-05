using System;

namespace Z21.Core.ResponseHandler.Programming
{
  public interface ICvNackResponseHandler : IZ21ResponseHandler
  {
    event EventHandler? OnCvNackReceived;
  }

  /// <summary>
  /// Sent when the decoder acknowledgement is missing during CV programming (<c>LAN_X_CV_NACK</c>,
  /// protocol §6.4).
  /// </summary>
  public class CvNackResponseHandler : ICvNackResponseHandler
  {
    public event EventHandler? OnCvNackReceived;

    public string Name => "LAN_X_CV_NACK";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 6, (2, 0x40), (3, 0x00), (4, 0x61), (5, 0x13));

    public void Handle(byte[] response)
    {
      OnCvNackReceived?.Invoke(this, EventArgs.Empty);
    }
  }
}
