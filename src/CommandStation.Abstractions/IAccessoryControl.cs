using System;
using System.Threading.Tasks;
using CommandStation.Model;

namespace CommandStation
{
  /// <summary>
  /// Switching turnouts and extended accessory decoders, with their status notifications.
  /// </summary>
  public interface IAccessoryControl
  {
    Task SetTurnoutAsync(ushort accessoryAddress, AccessoryOutput output, AccessoryState state, bool executeImmediately);

    Task SetExtAccessoryAsync(ushort accessoryAddress, byte payload);

    Task RequestTurnoutInfoAsync(ushort accessoryAddress);

    Task RequestExtAccessoryInfoAsync(ushort accessoryAddress);

    event EventHandler<TurnoutInfo>? TurnoutInfoReceived;

    event EventHandler<ExtAccessoryInfo>? ExtAccessoryInfoReceived;
  }
}
