using System;
using System.Linq;
using System.Threading.Tasks;
using CommandStation;
using CommandStation.Transport;
using Microsoft.Extensions.Logging;
using Z21.Core.Command;
using Z21.Core.Command.Driving;
using Z21.Core.Command.FastClock;
using Z21.Core.Command.Feedback;
using Z21.Core.Command.Programming;
using Z21.Core.Command.Switching;
using Z21.Core.Command.SystemState;
using Z21.Core.Command.SystemState.TrackPower;
using Z21.Core.Exception;
using Z21.Core.Helper;
using Z21.Core.Model;
using Z21.Core.ResponseHandler.Driving;
using Z21.Core.ResponseHandler.FastClock;
using Z21.Core.ResponseHandler.Feedback;
using Z21.Core.ResponseHandler.Programming;
using Z21.Core.ResponseHandler.Switching;
using Z21.Core.ResponseHandler.SystemState;
using Z21.Core.ResponseHandler.SystemState.TrackPower;

namespace Z21.Core
{
  public class Z21CommandStation : IZ21CommandStation, IProgrammingControl, IFeedbackControl, IFastClockControl, IDisposable
  {
    private readonly ITransport _transport;
    private readonly Z21ResponseHandler _dispatcher;
    private readonly Z21Options _options;
    private readonly DelayedAction _delayedKeepAliveAction;
    private readonly ILogger<Z21CommandStation>? _logger;
    private bool _disposed;

    /// <summary>
    /// IPv4 safe MTU for payload according to specification.
    /// </summary>
    public const int MaxUdpPayload = 1472;

    /// <exception cref="PlatformNotSupportedException">Thrown when system architecture is not little-endian.</exception>
    public Z21CommandStation(ITransport transport,
                             Z21ResponseHandler dispatcher,
                             IZ21CommandFactory commands,
                             Z21Options options,
                             ILocoInfoResponseHandler locoInfoResponseHandler,
                             ITurnoutInfoResponseHandler turnoutInfoResponseHandler,
                             IExtAccessoryInfoResponseHandler extAccessoryInfoResponseHandler,
                             ISystemStateDataChangedResponseHandler systemStateResponseHandler,
                             IFirmwareVersionResponseHandler firmwareVersionResponseHandler,
                             IStatusChangedResponseHandler statusChangedResponseHandler,
                             ITrackPowerOnResponseHandler trackPowerOnResponseHandler,
                             ITrackPowerOffResponseHandler trackPowerOffResponseHandler,
                             ICvResultResponseHandler cvResultResponseHandler,
                             ICvNackResponseHandler cvNackResponseHandler,
                             ICvNackShortCircuitResponseHandler cvNackShortCircuitResponseHandler,
                             IRmBusDataChangedResponseHandler rmBusDataChangedResponseHandler,
                             IFastClockDataResponseHandler fastClockDataResponseHandler,
                             ILogger<Z21CommandStation>? logger = null)
    {
      ArgumentNullException.ThrowIfNull(transport);
      ArgumentNullException.ThrowIfNull(dispatcher);
      ArgumentNullException.ThrowIfNull(commands);
      ArgumentNullException.ThrowIfNull(options);

      if (!BitConverter.IsLittleEndian)
        throw new PlatformNotSupportedException("Z21CommandStation requires little-endian architecture.");

      _transport = transport;
      _dispatcher = dispatcher;
      Commands = commands;
      _options = options;
      _logger = logger;
      _delayedKeepAliveAction = new(options.KeepAliveInterval, KeepAliveAsync);

      _transport.OnConnectionChanged += (_, args) =>
                                        {
                                          if (!args.IsConnected)
                                            _delayedKeepAliveAction.Stop();
                                          ConnectionChanged?.Invoke(this, args);
                                        };

      locoInfoResponseHandler.OnLocoInfoReceived += (_, args) => LocoInfoReceived?.Invoke(this, args.Data);
      turnoutInfoResponseHandler.OnTurnoutInfoReceived += (_, args) => TurnoutInfoReceived?.Invoke(this, new TurnoutInfo(args.AccessoryAddress, args.AccessoryOutput));
      extAccessoryInfoResponseHandler.OnExtAccessoryInfoReceived += (_, args) => ExtAccessoryInfoReceived?.Invoke(this, new ExtAccessoryInfo(args.AccessoryAddress, args.EncodedState, args.DataValid));
      systemStateResponseHandler.OnSystemStateDataChangedReceived += (_, args) => SystemStateReceived?.Invoke(this, args.SystemState);
      firmwareVersionResponseHandler.OnFirmwareVersionReceived += (_, args) => FirmwareVersionReceived?.Invoke(this, args.FirmwareVersion);
      statusChangedResponseHandler.OnStatusChangedReceived += (_, args) => StatusChanged?.Invoke(this, args.CentralState);
      trackPowerOnResponseHandler.OnTrackPowerOnReceived += (_, _) => TrackPowerChanged?.Invoke(this, true);
      trackPowerOffResponseHandler.OnTrackPowerOffReceived += (_, _) => TrackPowerChanged?.Invoke(this, false);
      cvResultResponseHandler.OnCvResultReceived += (_, args) => CvReadCompleted?.Invoke(this, new CvValue(args.CvAddress, args.Value));
      cvNackResponseHandler.OnCvNackReceived += (_, _) => CvProgrammingFailed?.Invoke(this, CvProgrammingError.NoAcknowledgement);
      cvNackShortCircuitResponseHandler.OnCvNackShortCircuitReceived += (_, _) => CvProgrammingFailed?.Invoke(this, CvProgrammingError.ShortCircuit);
      rmBusDataChangedResponseHandler.OnRmBusDataReceived += (_, args) => FeedbackChanged?.Invoke(this, new FeedbackData(args.GroupIndex, args.FeedbackStates));
      fastClockDataResponseHandler.OnFastClockDataReceived += (_, args) => ModelTimeChanged?.Invoke(this, new ModelTime(args.Data.Day, args.Data.Hour, args.Data.Minute, args.Data.Second, args.Data.Rate));
    }

    public IZ21CommandFactory Commands { get; }

    public bool IsConnected => _transport.IsConnected;

    public event EventHandler<ConnectionChangedEventArgs>? ConnectionChanged;
    public event EventHandler<LocoInfoData>? LocoInfoReceived;
    public event EventHandler<TurnoutInfo>? TurnoutInfoReceived;
    public event EventHandler<ExtAccessoryInfo>? ExtAccessoryInfoReceived;
    public event EventHandler<SystemState>? SystemStateReceived;
    public event EventHandler<FirmwareVersion>? FirmwareVersionReceived;
    public event EventHandler<CentralState>? StatusChanged;
    public event EventHandler<bool>? TrackPowerChanged;
    public event EventHandler<CvValue>? CvReadCompleted;
    public event EventHandler<CvProgrammingError>? CvProgrammingFailed;
    public event EventHandler<FeedbackData>? FeedbackChanged;
    public event EventHandler<ModelTime>? ModelTimeChanged;

    public async Task ConnectAsync()
    {
      _logger?.LogInformation("Z21CommandStation connecting.");
      await _transport.ConnectAsync();
      await LogOnAsync();
    }

    public Task DisconnectAsync()
    {
      _delayedKeepAliveAction.Stop();
      return _transport.DisconnectAsync();
    }

    public async Task SendCommandsAsync(params IZ21Command[] commands)
    {
      ArgumentNullException.ThrowIfNull(commands);

      if (!_transport.IsConnected)
        throw new NotConnectedException("Cannot send commands before ConnectAsync has completed.");

      foreach (var command in commands)
        _logger?.LogDebug("{commandName} sending {datagram} to Z21.", command.Name, BitConverter.ToString(command.Data));

      byte[] combinedPayload = commands.SelectMany(command => command.Data).ToArray();
      MtuPayloadLengthExceededException.ThrowIfExceeded(combinedPayload);

      await _transport.SendAsync(combinedPayload);
      _delayedKeepAliveAction.Delay();
    }

    public Task DriveAsync(ushort locoAddress, DccSpeedMode speedMode, DrivingDirection direction, ushort speed) =>
      SendCommandsAsync(Commands.Create<SetLocoDriveCommand>(speedMode, locoAddress, direction, speed));

    public Task EmergencyStopAsync(ushort locoAddress) => SendCommandsAsync(Commands.Create<SetLocoEStopCommand>(locoAddress));

    public Task SetFunctionAsync(ushort locoAddress, ushort functionIndex, FunctionToggleType toggleType) =>
      SendCommandsAsync(Commands.Create<SetLocoFunctionCommand>(locoAddress, functionIndex, toggleType));

    public Task PurgeAsync(ushort locoAddress) => SendCommandsAsync(Commands.Create<PurgeLocoCommand>(locoAddress));

    public Task RequestLocoInfoAsync(ushort locoAddress) => SendCommandsAsync(Commands.Create<GetLocoInfoCommand>(locoAddress));

    public Task SetTurnoutAsync(ushort accessoryAddress, AccessoryOutput output, AccessoryState state, bool executeImmediately) =>
      SendCommandsAsync(Commands.Create<SetTurnoutCommand>(accessoryAddress, output, state, executeImmediately));

    public Task SetExtAccessoryAsync(ushort accessoryAddress, byte payload) =>
      SendCommandsAsync(Commands.Create<SetExtAccessoryCommand>(accessoryAddress, payload));

    public Task RequestTurnoutInfoAsync(ushort accessoryAddress) => SendCommandsAsync(Commands.Create<GetTurnoutInfoCommand>(accessoryAddress));

    public Task RequestExtAccessoryInfoAsync(ushort accessoryAddress) => SendCommandsAsync(Commands.Create<GetExtAccessoryInfoCommand>(accessoryAddress));

    public Task TrackPowerOnAsync() => SendCommandsAsync(Commands.Create<SetTrackPowerOnCommand>());

    public Task TrackPowerOffAsync() => SendCommandsAsync(Commands.Create<SetTrackPowerOffCommand>());

    public Task EmergencyStopAllAsync() => SendCommandsAsync(Commands.Create<SetStopCommand>());

    public Task RequestSystemStateAsync() => SendCommandsAsync(Commands.Create<GetSystemStateDataCommand>());

    public Task RequestFirmwareVersionAsync() => SendCommandsAsync(Commands.Create<GetFirmwareVersionCommand>());

    public Task RequestStatusAsync() => SendCommandsAsync(Commands.Create<GetStatusCommand>());

    public Task ReadCvAsync(ushort cvAddress) => SendCommandsAsync(Commands.Create<CvReadCommand>(cvAddress));

    public Task WriteCvAsync(ushort cvAddress, byte value) => SendCommandsAsync(Commands.Create<CvWriteCommand>(cvAddress, value));

    public Task RequestFeedbackAsync(byte groupIndex) => SendCommandsAsync(Commands.Create<GetRmBusDataCommand>(groupIndex));

    public Task RequestModelTimeAsync() => SendCommandsAsync(Commands.Create<FastClockControlCommand>(FastClockAction.Read));

    public Task SetModelTimeAsync(ModelTime time) => SendCommandsAsync(Commands.Create<FastClockControlCommand>(time));

    public Task StartModelTimeAsync() => SendCommandsAsync(Commands.Create<FastClockControlCommand>(FastClockAction.Start));

    public Task StopModelTimeAsync() => SendCommandsAsync(Commands.Create<FastClockControlCommand>(FastClockAction.Stop));

    protected async virtual Task LogOnAsync() =>
      await SendCommandsAsync(Commands.Create<SetBroadcastFlagsCommand>(_options.BroadcastFlags), Commands.Create<GetFirmwareVersionCommand>());

    private async Task KeepAliveAsync()
    {
      try
      {
        await SendCommandsAsync(Commands.Create<GetFirmwareVersionCommand>());
      }
      catch (NotConnectedException exception)
      {
        _logger?.LogDebug(exception, "Keep-alive skipped because the station is not connected.");
      }
    }

    public void Dispose()
    {
      if (_disposed)
        return;

      _disposed = true;
      _delayedKeepAliveAction.Dispose();
      GC.SuppressFinalize(this);
    }
  }
}
