using System;

namespace Z21.Core.ResponseHandler.SystemState.TrackPower
{
  public interface IProgrammingModeResponseHandler : IZ21ResponseHandler
  {
    event EventHandler? OnProgrammingModeReceived;
  }

  /// <summary>
  /// The following packet is sent from the Z21 to the registered clients if the Z21 has been put into CV programming mode via LAN_X_CV_READ or LAN_X_CV_WRITE
  /// </summary>
  public class ProgrammingModeResponseHandler : IProgrammingModeResponseHandler
  {
    public event EventHandler? OnProgrammingModeReceived;

    public string Name => "LAN_X_BC_PROGRAMMING_MODE";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 6, (2, 0x40), (3, 0x00), (4, 0x61), (5, 0x02));

    public void Handle(byte[] response)
    {
      OnProgrammingModeReceived?.Invoke(this, EventArgs.Empty);
    }
  }
}