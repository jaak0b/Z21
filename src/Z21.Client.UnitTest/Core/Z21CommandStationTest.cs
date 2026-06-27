using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandStation.Model;
using Moq;
using Z21.Core;
using Z21.Core.Codecs;
using Z21.Core.Command;
using Z21.Core.Command.Driving;
using Z21.Core.Command.FastClock;
using Z21.Core.Command.Feedback;
using Z21.Core.Command.Programming;
using Z21.Core.Command.Switching;
using Z21.Core.Command.SystemState;
using Z21.Core.Command.SystemState.TrackPower;
using Z21.Core.Exception;
using Z21.Core.Framing;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.Driving;
using Z21.Core.ResponseHandler.FastClock;
using Z21.Core.ResponseHandler.Feedback;
using Z21.Core.ResponseHandler.Programming;
using Z21.Core.ResponseHandler.Switching;
using Z21.Core.ResponseHandler.SystemState;
using Z21.Core.ResponseHandler.SystemState.TrackPower;
using FastClockActionEnum = Z21.Core.Model.FastClockAction;

namespace Z21.UnitTest.Core
{
  public class Z21CommandStationTest
  {
    private FakeTransport _transport = null!;
    private IZ21CommandFactory _factory = null!;
    private Mock<ILocoInfoResponseHandler> _locoInfo = null!;
    private Mock<ITurnoutInfoResponseHandler> _turnoutInfo = null!;
    private Mock<IExtAccessoryInfoResponseHandler> _extAccessory = null!;
    private Mock<ISystemStateDataChangedResponseHandler> _systemState = null!;
    private Mock<IFirmwareVersionResponseHandler> _firmware = null!;
    private Mock<IStatusChangedResponseHandler> _statusChanged = null!;
    private Mock<ITrackPowerOnResponseHandler> _trackPowerOn = null!;
    private Mock<ITrackPowerOffResponseHandler> _trackPowerOff = null!;
    private Mock<ICvResultResponseHandler> _cvResult = null!;
    private Mock<ICvNackResponseHandler> _cvNack = null!;
    private Mock<ICvNackShortCircuitResponseHandler> _cvNackSc = null!;
    private Mock<IRmBusDataChangedResponseHandler> _rmBus = null!;
    private Mock<IFastClockDataResponseHandler> _fastClock = null!;
    private Z21CommandStation _station = null!;

    [SetUp]
    public void SetUp()
    {
      _transport = new FakeTransport();
      _factory = new Z21CommandFactory(new Z21FrameBuilder(), new AddressCodec(), new LocoSpeedCodec());
      _locoInfo = new Mock<ILocoInfoResponseHandler>();
      _turnoutInfo = new Mock<ITurnoutInfoResponseHandler>();
      _extAccessory = new Mock<IExtAccessoryInfoResponseHandler>();
      _systemState = new Mock<ISystemStateDataChangedResponseHandler>();
      _firmware = new Mock<IFirmwareVersionResponseHandler>();
      _statusChanged = new Mock<IStatusChangedResponseHandler>();
      _trackPowerOn = new Mock<ITrackPowerOnResponseHandler>();
      _trackPowerOff = new Mock<ITrackPowerOffResponseHandler>();
      _cvResult = new Mock<ICvResultResponseHandler>();
      _cvNack = new Mock<ICvNackResponseHandler>();
      _cvNackSc = new Mock<ICvNackShortCircuitResponseHandler>();
      _rmBus = new Mock<IRmBusDataChangedResponseHandler>();
      _fastClock = new Mock<IFastClockDataResponseHandler>();

      Z21ResponseHandler dispatcher = new(_transport, new Z21FrameReader(), new List<IZ21ResponseHandler>());

      _station = new Z21CommandStation(
                                       _transport,
                                       dispatcher,
                                       _factory,
                                       new Z21Options(),
                                       _locoInfo.Object,
                                       _turnoutInfo.Object,
                                       _extAccessory.Object,
                                       _systemState.Object,
                                       _firmware.Object,
                                       _statusChanged.Object,
                                       _trackPowerOn.Object,
                                       _trackPowerOff.Object,
                                       _cvResult.Object,
                                       _cvNack.Object,
                                       _cvNackSc.Object,
                                       _rmBus.Object,
                                       _fastClock.Object);
    }

    [TearDown]
    public void TearDown() => _station.Dispose();

    private static IEnumerable<TestCaseData> SendCases()
    {
      TestCaseData Case(string name, Func<Z21CommandStation, Task> invoke, Func<IZ21CommandFactory, IZ21Command> expected) =>
        new TestCaseData(invoke, expected).SetName(name);

      yield return Case("Drive", s => s.DriveAsync(3, DccSpeedMode.Steps128, DrivingDirection.Forward, 1),
                        f => f.Create<SetLocoDriveCommand>(DccSpeedMode.Steps128, (ushort)3, DrivingDirection.Forward, (ushort)1));
      yield return Case("EmergencyStop", s => s.EmergencyStopAsync(3), f => f.Create<SetLocoEStopCommand>((ushort)3));
      yield return Case("SetFunction", s => s.SetFunctionAsync(3, 1, FunctionToggleType.On),
                        f => f.Create<SetLocoFunctionCommand>((ushort)3, (ushort)1, FunctionToggleType.On));
      yield return Case("Purge", s => s.PurgeAsync(3), f => f.Create<PurgeLocoCommand>((ushort)3));
      yield return Case("RequestLocoInfo", s => s.RequestLocoInfoAsync(3), f => f.Create<GetLocoInfoCommand>((ushort)3));
      yield return Case("SetTurnout", s => s.SetTurnoutAsync(3, AccessoryOutput.Output1, AccessoryState.Activate, true),
                        f => f.Create<SetTurnoutCommand>((ushort)3, AccessoryOutput.Output1, AccessoryState.Activate, true));
      yield return Case("SetExtAccessory", s => s.SetExtAccessoryAsync(1, 5), f => f.Create<SetExtAccessoryCommand>((ushort)1, (byte)5));
      yield return Case("RequestTurnoutInfo", s => s.RequestTurnoutInfoAsync(3), f => f.Create<GetTurnoutInfoCommand>((ushort)3));
      yield return Case("RequestExtAccessoryInfo", s => s.RequestExtAccessoryInfoAsync(1), f => f.Create<GetExtAccessoryInfoCommand>((ushort)1));
      yield return Case("TrackPowerOn", s => s.TrackPowerOnAsync(), f => f.Create<SetTrackPowerOnCommand>());
      yield return Case("TrackPowerOff", s => s.TrackPowerOffAsync(), f => f.Create<SetTrackPowerOffCommand>());
      yield return Case("EmergencyStopAll", s => s.EmergencyStopAllAsync(), f => f.Create<SetStopCommand>());
      yield return Case("RequestSystemState", s => s.RequestSystemStateAsync(), f => f.Create<GetSystemStateDataCommand>());
      yield return Case("RequestFirmwareVersion", s => s.RequestFirmwareVersionAsync(), f => f.Create<GetFirmwareVersionCommand>());
      yield return Case("RequestStatus", s => s.RequestStatusAsync(), f => f.Create<GetStatusCommand>());
      yield return Case("ReadCv", s => s.ReadCvAsync(28), f => f.Create<CvReadCommand>((ushort)28));
      yield return Case("WriteCv", s => s.WriteCvAsync(28, 5), f => f.Create<CvWriteCommand>((ushort)28, (byte)5));
      yield return Case("RequestFeedback", s => s.RequestFeedbackAsync(1), f => f.Create<GetRmBusDataCommand>((byte)1));
      yield return Case("RequestModelTime", s => s.RequestModelTimeAsync(), f => f.Create<FastClockControlCommand>(FastClockActionEnum.Read));
      yield return Case("SetModelTime", s => s.SetModelTimeAsync(new ModelTime(0, 12, 30, 0, 8)),
                        f => f.Create<FastClockControlCommand>(new ModelTime(0, 12, 30, 0, 8)));
      yield return Case("StartModelTime", s => s.StartModelTimeAsync(), f => f.Create<FastClockControlCommand>(FastClockActionEnum.Start));
      yield return Case("StopModelTime", s => s.StopModelTimeAsync(), f => f.Create<FastClockControlCommand>(FastClockActionEnum.Stop));
    }

    [TestCaseSource(nameof(SendCases))]
    public async Task SendMethods_SendExpectedDatagram(Func<Z21CommandStation, Task> invoke, Func<IZ21CommandFactory, IZ21Command> expected)
    {
      await _station.ConnectAsync();
      _transport.Sent.Clear();

      await invoke(_station);

      Assert.That(_transport.Sent.Single(), Is.EqualTo(expected(_factory).Data));
    }

    [Test]
    public void SendCommandsAsync_WhenNotConnected_ThrowsNotConnectedException()
    {
      Assert.ThrowsAsync<NotConnectedException>(() => _station.DriveAsync(3, DccSpeedMode.Steps128, DrivingDirection.Forward, 1));
      Assert.That(_transport.IsConnected, Is.False, "send must not implicitly connect");
    }

    [Test]
    public async Task ConnectionLost_StopsKeepAlive_NoSpuriousSends()
    {
      // Build a station with a fast keep-alive so the timer would fire well within the test window.
      Z21ResponseHandler dispatcher = new(_transport, new Z21FrameReader(), new List<IZ21ResponseHandler>());
      using Z21CommandStation station = new(
                                            _transport,
                                            dispatcher,
                                            _factory,
                                            new Z21Options { KeepAliveInterval = TimeSpan.FromMilliseconds(100) },
                                            _locoInfo.Object, _turnoutInfo.Object, _extAccessory.Object, _systemState.Object,
                                            _firmware.Object, _statusChanged.Object, _trackPowerOn.Object, _trackPowerOff.Object,
                                            _cvResult.Object, _cvNack.Object, _cvNackSc.Object, _rmBus.Object, _fastClock.Object);

      await station.ConnectAsync(); // arms the keep-alive timer
      _transport.RaiseConnectionLost(); // socket-level loss must stop the keep-alive
      _transport.SetConnected(true); // transport reconnects on its own; the station did not re-arm anything
      _transport.Sent.Clear();

      await Task.Delay(TimeSpan.FromMilliseconds(500)); // > 4 keep-alive intervals

      Assert.That(_transport.Sent, Is.Empty, "a lost connection must stop the keep-alive timer (no spurious keep-alive sends)");
    }

    [Test]
    public async Task SendAfterDisconnect_ThrowsAndDoesNotReconnect()
    {
      await _station.ConnectAsync();
      await _station.DisconnectAsync();

      Assert.ThrowsAsync<NotConnectedException>(() => _station.TrackPowerOnAsync());
      Assert.That(_transport.IsConnected, Is.False, "disconnect must be authoritative");
    }

    [Test]
    public async Task DisconnectAsync_SendsLogOffThenDisconnects()
    {
      await _station.ConnectAsync();
      _transport.Sent.Clear();

      await _station.DisconnectAsync();

      byte[] expected = _factory.Create<LogOffCommand>().Data;
      Assert.That(_transport.Sent.Single(), Is.EqualTo(expected),
                  "disconnect should send LAN_LOGOFF so the Z21 frees the client slot immediately");
      Assert.That(_transport.IsConnected, Is.False, "transport must be disconnected after DisconnectAsync");
    }

    [Test]
    public async Task DisconnectAsync_WhenNotConnected_DoesNotSendLogOff()
    {
      await _station.DisconnectAsync();

      Assert.That(_transport.Sent, Is.Empty, "no LAN_LOGOFF should be sent when the transport was never connected");
    }

    [Test]
    public async Task DisconnectAsync_WhenLogOffSendFails_StillDisconnects()
    {
      await _station.ConnectAsync();
      _transport.ThrowOnSend = true;

      await _station.DisconnectAsync();

      Assert.That(_transport.IsConnected, Is.False, "disconnect must complete even if the LAN_LOGOFF send fails");
    }

    [Test]
    public async Task ConnectAsync_SendsLogonAndSetsConnected()
    {
      await _station.ConnectAsync();

      Assert.That(_station.IsConnected, Is.True);
      byte[] expected = _factory.Create<SetBroadcastFlagsCommand>(new Z21Options().BroadcastFlags).Data
                                .Concat(_factory.Create<GetFirmwareVersionCommand>().Data).ToArray();
      Assert.That(_transport.Sent.Single(), Is.EqualTo(expected), "logon should send broadcast flags + firmware query in one packet");
    }

    [Test]
    public void LocoInfoReceived_FromHandler_IsReRaisedWithSameData()
    {
      LocoInfoData data = new()
                          {
                            LocoAddress = 3,
                            LocoFunctionsData = new List<LocoFunctionData>(),
                            DccSpeedMode = DccSpeedMode.Steps128,
                            DecoderMode = DecoderMode.DCC,
                            DrivingDirection = DrivingDirection.Forward,
                            LocoSpeed = 1,
                            LocoIsBusy = false,
                            LocoContainedInDoubleTraction = false,
                            SmartSearch = false
                          };
      LocoInfoData? received = null;
      _station.LocoInfoReceived += (_, d) => received = d;

      _locoInfo.Raise(handler => handler.OnLocoInfoReceived += null, new LocoInfoReceivedEventArgs(data));

      Assert.That(received, Is.SameAs(data));
    }

    [Test]
    public void TurnoutInfoReceived_FromHandler_IsReRaised()
    {
      TurnoutInfo? received = null;
      _station.TurnoutInfoReceived += (_, info) => received = info;

      _turnoutInfo.Raise(h => h.OnTurnoutInfoReceived += null, new TurnoutInfoReceivedEventArgs(65, AccessoryOutput.Output1));

      Assert.That(received, Is.EqualTo(new TurnoutInfo(65, AccessoryOutput.Output1)));
    }

    [Test]
    public void ExtAccessoryInfoReceived_FromHandler_IsReRaised()
    {
      ExtAccessoryInfo? received = null;
      _station.ExtAccessoryInfoReceived += (_, info) => received = info;

      _extAccessory.Raise(h => h.OnExtAccessoryInfoReceived += null, new ExtAccessoryInfoReceivedEventArgs(1, 5, true));

      Assert.That(received, Is.EqualTo(new ExtAccessoryInfo(1, 5, true)));
    }

    [Test]
    public void SystemStateReceived_FromHandler_IsReRaised()
    {
      SystemState state = new() { CentralState = new CentralState(), CentralStateEx = new CentralStateEx() };
      SystemState? received = null;
      _station.SystemStateReceived += (_, s) => received = s;

      _systemState.Raise(h => h.OnSystemStateDataChangedReceived += null, _systemState.Object, new SystemStatusChangedReceivedEventArgs(state));

      Assert.That(received, Is.SameAs(state));
    }

    [Test]
    public void FirmwareVersionReceived_FromHandler_IsReRaised()
    {
      FirmwareVersion version = new(1, 42);
      FirmwareVersion? received = null;
      _station.FirmwareVersionReceived += (_, v) => received = v;

      _firmware.Raise(h => h.OnFirmwareVersionReceived += null, _firmware.Object, new FirmwareVersionReceivedEventArgs(version));

      Assert.That(received, Is.SameAs(version));
    }

    [Test]
    public void StatusChanged_FromHandler_IsReRaised()
    {
      CentralState? received = null;
      _station.StatusChanged += (_, s) => received = s;

      _statusChanged.Raise(h => h.OnStatusChangedReceived += null, _statusChanged.Object, new StatusChangedReceivedEventArgs(new CentralState()));

      Assert.That(received, Is.Not.Null);
    }

    [Test]
    public void TrackPowerChanged_OnTrackPowerOn_RaisesTrue()
    {
      bool? state = null;
      _station.TrackPowerChanged += (_, on) => state = on;

      _trackPowerOn.Raise(handler => handler.OnTrackPowerOnReceived += null, System.EventArgs.Empty);

      Assert.That(state, Is.True);
    }

    [Test]
    public void TrackPowerChanged_OnTrackPowerOff_RaisesFalse()
    {
      bool? state = null;
      _station.TrackPowerChanged += (_, on) => state = on;

      _trackPowerOff.Raise(handler => handler.OnTrackPowerOffReceived += null, System.EventArgs.Empty);

      Assert.That(state, Is.False);
    }

    [Test]
    public async Task ReadCvAsync_SendsCvReadDatagram()
    {
      await _station.ConnectAsync();
      _transport.Sent.Clear();

      await ((CommandStation.IProgrammingControl)_station).ReadCvAsync(28);

      byte[] expected = _factory.Create<CvReadCommand>((ushort)28).Data;
      Assert.That(_transport.Sent.Single(), Is.EqualTo(expected));
    }

    [Test]
    public void CvReadCompleted_FromHandler_IsReRaisedAsCvValue()
    {
      CvValue? received = null;
      ((CommandStation.IProgrammingControl)_station).CvReadCompleted += (_, value) => received = value;

      _cvResult.Raise(handler => handler.OnCvResultReceived += null, new CvResultReceivedEventArgs(28, 5));

      Assert.That(received, Is.EqualTo(new CvValue(28, 5)));
    }

    [Test]
    public void CvProgrammingFailed_OnNack_RaisesNoAcknowledgement()
    {
      CvProgrammingError? error = null;
      ((CommandStation.IProgrammingControl)_station).CvProgrammingFailed += (_, e) => error = e;

      _cvNack.Raise(handler => handler.OnCvNackReceived += null, System.EventArgs.Empty);

      Assert.That(error, Is.EqualTo(CvProgrammingError.NoAcknowledgement));
    }

    [Test]
    public void CvProgrammingFailed_OnShortCircuit_RaisesShortCircuitError()
    {
      CvProgrammingError? error = null;
      ((CommandStation.IProgrammingControl)_station).CvProgrammingFailed += (_, e) => error = e;

      _cvNackSc.Raise(handler => handler.OnCvNackShortCircuitReceived += null, System.EventArgs.Empty);

      Assert.That(error, Is.EqualTo(CvProgrammingError.ShortCircuit));
    }

    [Test]
    public void FeedbackChanged_FromHandler_IsReRaised()
    {
      FeedbackData? received = null;
      ((CommandStation.IFeedbackControl)_station).FeedbackChanged += (_, data) => received = data;

      _rmBus.Raise(h => h.OnRmBusDataReceived += null, new RmBusDataReceivedEventArgs(1, new byte[] { 0x05 }));

      Assert.Multiple(() =>
                      {
                        Assert.That(received!.GroupIndex, Is.EqualTo(1));
                        Assert.That(received.States, Is.EqualTo(new byte[] { 0x05 }));
                      });
    }

    [Test]
    public void ModelTimeChanged_FromHandler_IsReRaised()
    {
      ModelTime? received = null;
      ((CommandStation.IFastClockControl)_station).ModelTimeChanged += (_, time) => received = time;

      FastClockData data = new(0, 12, 30, 45, 8, false, false, FastClockSettings.Enabled);
      _fastClock.Raise(h => h.OnFastClockDataReceived += null, new FastClockDataReceivedEventArgs(data));

      Assert.Multiple(() =>
                      {
                        Assert.That(received!.Hour, Is.EqualTo(12));
                        Assert.That(received.Minute, Is.EqualTo(30));
                        Assert.That(received.Second, Is.EqualTo(45));
                        Assert.That(received.Rate, Is.EqualTo(8));
                      });
    }
  }
}
