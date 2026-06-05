using System;
using System.Threading.Tasks;
using CommandStation.Model;

namespace CommandStation
{
  /// <summary>
  /// Querying command-station system information and receiving status notifications.
  /// </summary>
  public interface ISystemInfoProvider
  {
    Task RequestSystemStateAsync();

    Task RequestFirmwareVersionAsync();

    Task RequestStatusAsync();

    event EventHandler<SystemState>? SystemStateReceived;

    event EventHandler<FirmwareVersion>? FirmwareVersionReceived;

    event EventHandler<CentralState>? StatusChanged;
  }
}
