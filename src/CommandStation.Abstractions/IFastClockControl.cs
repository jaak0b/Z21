using System;
using System.Threading.Tasks;
using CommandStation.Model;

namespace CommandStation
{
  /// <summary>
  /// Controlling the accelerated model railway clock (model time).
  /// </summary>
  public interface IFastClockControl
  {
    Task RequestModelTimeAsync();

    Task SetModelTimeAsync(ModelTime time);

    Task StartModelTimeAsync();

    Task StopModelTimeAsync();

    event EventHandler<ModelTime>? ModelTimeChanged;
  }
}
