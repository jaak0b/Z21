using System;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Booster
{
  public interface IBoosterSystemStateResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<BoosterSystemStateReceivedEventArgs>? OnBoosterSystemStateReceived;
  }

  /// <summary>
  /// Reports a zLink booster system state (<c>LAN_BOOSTER_SYSTEMSTATE_DATACHANGED</c>, protocol §11.2.4).
  /// </summary>
  public class BoosterSystemStateResponseHandler : IBoosterSystemStateResponseHandler
  {
    public event EventHandler<BoosterSystemStateReceivedEventArgs>? OnBoosterSystemStateReceived;

    public string Name => "LAN_BOOSTER_SYSTEMSTATE_DATACHANGED";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 28, (2, 0xBA), (3, 0x00));

    public void Handle(byte[] response)
    {
      BoosterSystemState state = new(
                                     BitConverter.ToInt16(response, 4),
                                     BitConverter.ToInt16(response, 6),
                                     BitConverter.ToInt16(response, 8),
                                     BitConverter.ToInt16(response, 10),
                                     BitConverter.ToInt16(response, 12),
                                     BitConverter.ToInt16(response, 14),
                                     BitConverter.ToUInt16(response, 16),
                                     BitConverter.ToUInt16(response, 18),
                                     BitConverter.ToUInt16(response, 20),
                                     response[22],
                                     response[23],
                                     response[24],
                                     response[26]);
      OnBoosterSystemStateReceived?.Invoke(this, new(state));
    }
  }
}
