using System;
using System.Threading.Tasks;
using CommandStation.Transport;

namespace CommandStation
{
  /// <summary>
  /// A protocol-agnostic connection to a model-railway command station. Feature operations live on
  /// the capability interfaces (<see cref="ILocoControl"/>, <see cref="IAccessoryControl"/>,
  /// <see cref="ITrackPowerControl"/>, <see cref="ISystemInfoProvider"/>); a station implements only
  /// the capabilities it supports, so consumers test for them (e.g. <c>station is ILocoControl</c>).
  /// </summary>
  public interface ICommandStation
  {
    bool IsConnected { get; }

    event EventHandler<ConnectionChangedEventArgs>? ConnectionChanged;

    Task ConnectAsync();

    Task DisconnectAsync();
  }
}
