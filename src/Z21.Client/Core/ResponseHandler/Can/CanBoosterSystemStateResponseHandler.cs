using System;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Can
{
  public interface ICanBoosterSystemStateResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<CanBoosterSystemStateReceivedEventArgs>? OnCanBoosterSystemStateReceived;
  }

  /// <summary>
  /// From Z21 FW version 1.41, reports a CAN booster system state
  /// (<c>LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD</c>, protocol §10.2.3).
  /// </summary>
  public class CanBoosterSystemStateResponseHandler : ICanBoosterSystemStateResponseHandler
  {
    public event EventHandler<CanBoosterSystemStateReceivedEventArgs>? OnCanBoosterSystemStateReceived;

    public string Name => "LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD";

    public bool CanHandle(byte[] response)
    {
      ArgumentNullException.ThrowIfNull(response);
      return response.Length >= 14 && response[2] == 0xCA && response[3] == 0x00;
    }

    public void Handle(byte[] response)
    {
      ushort networkId = BitConverter.ToUInt16(response, 4);
      ushort outputPort = BitConverter.ToUInt16(response, 6);
      CanBoosterState state = (CanBoosterState)BitConverter.ToUInt16(response, 8);
      ushort vccVoltage = BitConverter.ToUInt16(response, 10);
      ushort current = BitConverter.ToUInt16(response, 12);
      OnCanBoosterSystemStateReceived?.Invoke(this, new(new CanBoosterSystemState(networkId, outputPort, state, vccVoltage, current)));
    }
  }
}
