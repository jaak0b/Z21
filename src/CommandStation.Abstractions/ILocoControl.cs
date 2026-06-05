using System;
using System.Threading.Tasks;
using CommandStation.Model;

namespace CommandStation
{
  /// <summary>
  /// Driving a locomotive: speed, direction, functions, and locomotive status notifications.
  /// </summary>
  public interface ILocoControl
  {
    Task DriveAsync(ushort locoAddress, DccSpeedMode speedMode, DrivingDirection direction, ushort speed);

    Task EmergencyStopAsync(ushort locoAddress);

    Task SetFunctionAsync(ushort locoAddress, ushort functionIndex, FunctionToggleType toggleType);

    Task PurgeAsync(ushort locoAddress);

    Task RequestLocoInfoAsync(ushort locoAddress);

    event EventHandler<LocoInfoData>? LocoInfoReceived;
  }
}
