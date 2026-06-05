using System.Threading.Tasks;
using CommandStation;
using Z21.Core.Command;

namespace Z21.Core
{
  /// <summary>
  /// The Z21 command station: the protocol-agnostic capabilities plus a Z21-specific raw escape hatch
  /// for sending hand-built commands.
  /// </summary>
  public interface IZ21CommandStation : ICommandStation, ILocoControl, IAccessoryControl, ITrackPowerControl, ISystemInfoProvider, IProgrammingControl, IFeedbackControl, IFastClockControl
  {
    /// <summary>
    /// Factory for building raw Z21 commands to pass to <see cref="SendCommandsAsync"/>.
    /// </summary>
    IZ21CommandFactory Commands { get; }

    /// <summary>
    /// Sends one or more raw commands in a single UDP packet.
    /// </summary>
    Task SendCommandsAsync(params IZ21Command[] commands);
  }
}
