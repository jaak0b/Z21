using System;

namespace Z21.Core.ResponseHandler.Programming
{
  public interface ICvNackShortCircuitResponseHandler : IZ21ResponseHandler
  {
    event EventHandler? OnCvNackShortCircuitReceived;
  }

  /// <summary>
  /// Sent when CV programming fails because of a short circuit on the track (<c>LAN_X_CV_NACK_SC</c>,
  /// protocol §6.3).
  /// </summary>
  public class CvNackShortCircuitResponseHandler : ICvNackShortCircuitResponseHandler
  {
    public event EventHandler? OnCvNackShortCircuitReceived;

    public string Name => "LAN_X_CV_NACK_SC";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 6, (2, 0x40), (3, 0x00), (4, 0x61), (5, 0x12));

    public void Handle(byte[] response)
    {
      OnCvNackShortCircuitReceived?.Invoke(this, EventArgs.Empty);
    }
  }
}
