using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CommandStation.Model;
using Microsoft.Extensions.DependencyInjection;
using Z21.Core;
using Z21.Core.Command.SystemState;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.SystemState;
using Z21.DependencyInjection;

namespace Z21.SmokeTest
{
  /// <summary>
  /// End-to-end smoke tests that exercise the library against a real Z21 on the network. These are
  /// <see cref="ExplicitAttribute"/> and tagged <c>Hardware</c>, so the normal <c>dotnet test</c> /
  /// CI sweep discovers but never runs them. Run them on demand with a live command station:
  /// <code>
  /// $env:Z21_ENDPOINT="192.168.0.111:21105"; $env:Z21_LOCO="3"
  /// dotnet test src/Z21.SmokeTest --filter "TestCategory=Hardware"
  /// </code>
  /// Set <c>Z21_READONLY=1</c> to skip the destructive (track power / driving / turnout) tests.
  /// </summary>
  [TestFixture]
  [Category("Hardware")]
  [Explicit("Requires a live Z21 on the network; set Z21_ENDPOINT to run.")]
  public class Z21HardwareTests
  {
    private readonly TimeSpan _eventTimeout = TimeSpan.FromSeconds(5);
    private readonly TimeSpan _settleDelay = TimeSpan.FromMilliseconds(300);

    private ServiceProvider _provider = null!;
    private IZ21CommandStation _station = null!;
    private ushort _locoAddress;
    private bool _readOnly;

    [OneTimeSetUp]
    public async Task ConnectToZ21()
    {
      string? endpoint = Environment.GetEnvironmentVariable("Z21_ENDPOINT");
      if (string.IsNullOrWhiteSpace(endpoint))
        Assert.Ignore("Set Z21_ENDPOINT (e.g. 192.168.0.111:21105) to run the Z21 hardware tests.");

      string[] hostPort = endpoint!.Split(':');
      IPAddress ip = IPAddress.Parse(hostPort[0]);
      int port = hostPort.Length > 1 ? int.Parse(hostPort[1]) : 21105;
      _locoAddress = ushort.TryParse(Environment.GetEnvironmentVariable("Z21_LOCO"), out ushort loco) ? loco : (ushort)3;
      _readOnly = IsTruthy(Environment.GetEnvironmentVariable("Z21_READONLY"));

      var services = new ServiceCollection();
      services.AddZ21(
        t => t.RemoteEndPoint = new IPEndPoint(ip, port),
        o => o.BroadcastFlags =
        [
          Z21BroadcastFlags.DriveAndSwitchingMessages,
          Z21BroadcastFlags.LocoInfoChangedMessages,
          Z21BroadcastFlags.SystemStateDataChangedMessages,
        ]);

      _provider = services.BuildServiceProvider();
      _station = _provider.GetRequiredService<IZ21CommandStation>();

      await _station.ConnectAsync();

      Task<FirmwareVersion> firmware = NextEventAsync<FirmwareVersion>(
        h => _station.FirmwareVersionReceived += h,
        h => _station.FirmwareVersionReceived -= h,
        _eventTimeout);
      await _station.RequestFirmwareVersionAsync();
      await firmware;

      Assert.That(_station.IsConnected, Is.True, "Station did not connect to the Z21.");
    }

    [OneTimeTearDown]
    public async Task DisconnectFromZ21()
    {
      if (_station is not null && _station.IsConnected)
      {
        if (!_readOnly)
        {
          await _station.DriveAsync(_locoAddress, DccSpeedMode.Steps128, DrivingDirection.Forward, 0);
          await _station.TrackPowerOffAsync();
        }
        await _station.DisconnectAsync();
      }

      if (_provider is not null)
        await _provider.DisposeAsync();
    }

    [Test]
    [Order(1)]
    public void Connects_AndReportsConnected()
      => Assert.That(_station.IsConnected, Is.True);

    [Test]
    [Order(2)]
    public async Task SerialNumber_IsReported()
    {
      ISerialNumberResponseHandler handler = _provider.GetRequiredService<ISerialNumberResponseHandler>();
      Task<SerialNumberReceivedEventArgs> serial = NextEventAsync<SerialNumberReceivedEventArgs>(
        h => handler.OnSerialNumberReceived += h,
        h => handler.OnSerialNumberReceived -= h,
        _eventTimeout);

      await _station.SendCommandsAsync(_station.Commands.Create<GetSerialNumberCommand>());

      SerialNumberReceivedEventArgs args = await serial;
      Assert.That(args.SerialNumber, Is.GreaterThan(0u));
    }

    [Test]
    [Order(3)]
    public async Task HardwareInfo_IsReported()
    {
      IHardwareInfoResponseHandler handler = _provider.GetRequiredService<IHardwareInfoResponseHandler>();
      Task<HardwareInfoEventArgs> hardware = NextEventAsync<HardwareInfoEventArgs>(
        h => handler.OnHardwareInfoReceived += h,
        h => handler.OnHardwareInfoReceived -= h,
        _eventTimeout);

      await _station.SendCommandsAsync(_station.Commands.Create<GetHardwareInfoCommand>());

      HardwareInfoEventArgs args = await hardware;
      Assert.Multiple(() =>
      {
        Assert.That(args.Z21HardwareType, Is.GreaterThan(0));
        Assert.That(args.FirmwareVersion, Is.GreaterThan(0));
      });
    }

    [Test]
    [Order(4)]
    public async Task FirmwareVersion_IsReported()
    {
      Task<FirmwareVersion> firmware = NextEventAsync<FirmwareVersion>(
        h => _station.FirmwareVersionReceived += h,
        h => _station.FirmwareVersionReceived -= h,
        _eventTimeout);

      await _station.RequestFirmwareVersionAsync();

      FirmwareVersion version = await firmware;
      Assert.That(version.Major, Is.GreaterThan(0));
    }

    [Test]
    [Order(5)]
    public async Task XBusVersion_IsReported()
    {
      IVersionResponseHandler handler = _provider.GetRequiredService<IVersionResponseHandler>();
      Task<VersionReceivedEventArgs> version = NextEventAsync<VersionReceivedEventArgs>(
        h => handler.OnVersionReceived += h,
        h => handler.OnVersionReceived -= h,
        _eventTimeout);

      await _station.SendCommandsAsync(_station.Commands.Create<GetVersionCommand>());

      VersionReceivedEventArgs args = await version;
      Assert.That(args.CommandStationId, Is.GreaterThan(0));
    }

    [Test]
    [Order(6)]
    public async Task BroadcastFlags_AreReported()
    {
      IBroadcastFlagsResponseHandler handler = _provider.GetRequiredService<IBroadcastFlagsResponseHandler>();
      Task<BroadcastFlagsReceivedEventArgs> flags = NextEventAsync<BroadcastFlagsReceivedEventArgs>(
        h => handler.OnBroadcastFlagsReceived += h,
        h => handler.OnBroadcastFlagsReceived -= h,
        _eventTimeout);

      await _station.SendCommandsAsync(_station.Commands.Create<GetBroadcastFlagsCommand>());

      BroadcastFlagsReceivedEventArgs args = await flags;
      Assert.That(args.BroadCastFlag, Is.Not.Zero);
    }

    [Test]
    [Order(7)]
    public async Task SystemState_IsReported()
    {
      Task<SystemState> state = NextEventAsync<SystemState>(
        h => _station.SystemStateReceived += h,
        h => _station.SystemStateReceived -= h,
        _eventTimeout);

      await _station.RequestSystemStateAsync();

      SystemState systemState = await state;
      Assert.Multiple(() =>
      {
        Assert.That(systemState.CentralState, Is.Not.Null);
        Assert.That(systemState.SupplyVoltage, Is.GreaterThan(0));
      });
    }

    [Test]
    [Order(8)]
    public async Task Status_IsReported()
    {
      Task<CentralState> status = NextEventAsync<CentralState>(
        h => _station.StatusChanged += h,
        h => _station.StatusChanged -= h,
        _eventTimeout);

      await _station.RequestStatusAsync();

      CentralState centralState = await status;
      Assert.That(centralState, Is.Not.Null);
    }

    [Test]
    [Order(10)]
    public async Task TrackPower_OnOffOn_RaisesTrackPowerChanged()
    {
      SkipIfReadOnly();

      await _station.TrackPowerOffAsync();
      await Task.Delay(_settleDelay);

      Assert.That(await ExpectPowerChangeAsync(true), Is.True);
      Assert.That(await ExpectPowerChangeAsync(false), Is.False);
      Assert.That(await ExpectPowerChangeAsync(true), Is.True);
    }

    [Test]
    [Order(11)]
    public async Task Drive_RampForward_ReportsSpeedAndDirection()
    {
      SkipIfReadOnly();
      await EnsurePowerOnAsync();

      await _station.DriveAsync(_locoAddress, DccSpeedMode.Steps128, DrivingDirection.Forward, 10);
      await _station.DriveAsync(_locoAddress, DccSpeedMode.Steps128, DrivingDirection.Forward, 40);
      await _station.DriveAsync(_locoAddress, DccSpeedMode.Steps128, DrivingDirection.Forward, 80);

      LocoInfoData info = await RequestLocoInfoAsync();
      Assert.Multiple(() =>
      {
        Assert.That(info.LocoAddress, Is.EqualTo(_locoAddress));
        Assert.That(info.DrivingDirection, Is.EqualTo(DrivingDirection.Forward));
        Assert.That(info.LocoSpeed, Is.GreaterThan(0));
      });
    }

    [Test]
    [Order(12)]
    public async Task Functions_F0F1_Toggle()
    {
      SkipIfReadOnly();
      await EnsurePowerOnAsync();

      await _station.SetFunctionAsync(_locoAddress, 0, FunctionToggleType.On);
      LocoInfoData onInfo = await RequestLocoInfoAsync();
      Assert.That(
        onInfo.LocoFunctionsData.Any(f => f.FunctionIndex == 0 && f.FunctionToggleType == FunctionToggleType.On),
        Is.True,
        "F0 (lights) should be reported as On.");

      await _station.SetFunctionAsync(_locoAddress, 1, FunctionToggleType.On);
      await _station.SetFunctionAsync(_locoAddress, 1, FunctionToggleType.Off);
      LocoInfoData offInfo = await RequestLocoInfoAsync();
      Assert.That(
        offInfo.LocoFunctionsData.Any(f => f.FunctionIndex == 1 && f.FunctionToggleType == FunctionToggleType.Off),
        Is.True,
        "F1 should be reported as Off after toggling on then off.");
    }

    [Test]
    [Order(13)]
    public async Task Drive_Reverse_ReportsBackward()
    {
      SkipIfReadOnly();
      await EnsurePowerOnAsync();

      await _station.DriveAsync(_locoAddress, DccSpeedMode.Steps128, DrivingDirection.Backward, 30);

      LocoInfoData info = await RequestLocoInfoAsync();
      Assert.That(info.DrivingDirection, Is.EqualTo(DrivingDirection.Backward));
    }

    [Test]
    [Order(14)]
    public async Task EmergencyStop_StopsLoco()
    {
      SkipIfReadOnly();
      await EnsurePowerOnAsync();

      await _station.DriveAsync(_locoAddress, DccSpeedMode.Steps128, DrivingDirection.Forward, 50);
      await _station.EmergencyStopAsync(_locoAddress);

      LocoInfoData info = await RequestLocoInfoAsync();
      Assert.That(info.LocoSpeed, Is.Zero);
    }

    [Test]
    [Order(15)]
    public async Task Turnout_ActivateDeactivateRead_RaisesTurnoutInfo()
    {
      SkipIfReadOnly();
      await EnsurePowerOnAsync();

      await _station.SetTurnoutAsync(1, AccessoryOutput.Output1, AccessoryState.Activate, true);
      await _station.SetTurnoutAsync(1, AccessoryOutput.Output1, AccessoryState.Deactivate, true);

      Task<TurnoutInfo> turnout = NextEventAsync<TurnoutInfo>(
        h => _station.TurnoutInfoReceived += h,
        h => _station.TurnoutInfoReceived -= h,
        _eventTimeout,
        t => t.AccessoryAddress == 1);
      await _station.RequestTurnoutInfoAsync(1);

      TurnoutInfo info = await turnout;
      Assert.That(info.AccessoryAddress, Is.EqualTo((ushort)1));
    }

    private void SkipIfReadOnly()
    {
      if (_readOnly)
        Assert.Ignore("Z21_READONLY is set; skipping track power, driving and turnout tests.");
    }

    private bool IsTruthy(string? value) =>
      value is not null && (value == "1"
        || string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
        || string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase));

    private async Task EnsurePowerOnAsync()
    {
      await _station.TrackPowerOnAsync();
      await Task.Delay(_settleDelay);
    }

    private async Task<bool> ExpectPowerChangeAsync(bool on)
    {
      Task<bool> change = NextEventAsync<bool>(
        h => _station.TrackPowerChanged += h,
        h => _station.TrackPowerChanged -= h,
        _eventTimeout,
        state => state == on);

      if (on)
        await _station.TrackPowerOnAsync();
      else
        await _station.TrackPowerOffAsync();

      return await change;
    }

    private async Task<LocoInfoData> RequestLocoInfoAsync()
    {
      Task<LocoInfoData> info = NextEventAsync<LocoInfoData>(
        h => _station.LocoInfoReceived += h,
        h => _station.LocoInfoReceived -= h,
        _eventTimeout,
        data => data.LocoAddress == _locoAddress);

      await _station.RequestLocoInfoAsync(_locoAddress);

      return await info;
    }

    private async Task<TArgs> NextEventAsync<TArgs>(
      Action<EventHandler<TArgs>> subscribe,
      Action<EventHandler<TArgs>> unsubscribe,
      TimeSpan timeout,
      Func<TArgs, bool>? predicate = null)
    {
      var completion = new TaskCompletionSource<TArgs>(TaskCreationOptions.RunContinuationsAsynchronously);
      EventHandler<TArgs> handler = (_, args) =>
      {
        if (predicate is null || predicate(args))
          completion.TrySetResult(args);
      };

      subscribe(handler);
      try
      {
        using var cancellation = new CancellationTokenSource(timeout);
        await using (cancellation.Token.Register(() => completion.TrySetCanceled(cancellation.Token)))
        {
          try
          {
            return await completion.Task;
          }
          catch (OperationCanceledException)
          {
            throw new TimeoutException($"No matching {typeof(TArgs).Name} event was received within {timeout.TotalSeconds:0.#}s.");
          }
        }
      }
      finally
      {
        unsubscribe(handler);
      }
    }
  }
}
