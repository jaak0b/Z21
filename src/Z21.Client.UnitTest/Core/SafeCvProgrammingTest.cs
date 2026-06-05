using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommandStation;
using Moq;
using Z21.Core;
using Z21.Core.Codecs;
using Z21.Core.Command;
using Z21.Core.Framing;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.Driving;
using Z21.Core.ResponseHandler.FastClock;
using Z21.Core.ResponseHandler.Feedback;
using Z21.Core.ResponseHandler.Programming;
using Z21.Core.ResponseHandler.Switching;
using Z21.Core.ResponseHandler.SystemState;
using Z21.Core.ResponseHandler.SystemState.TrackPower;

namespace Z21.UnitTest.Core
{
  public class SafeCvProgrammingTest
  {
    private FakeTransport _transport = null!;
    private Mock<ICvResultResponseHandler> _cvResult = null!;
    private Mock<ICvNackResponseHandler> _cvNack = null!;
    private Mock<ICvNackShortCircuitResponseHandler> _cvNackSc = null!;
    private Z21CommandStation _station = null!;

    [SetUp]
    public void SetUp()
    {
      _transport = new FakeTransport();
      Z21CommandFactory factory = new(new Z21FrameBuilder(), new AddressCodec(), new LocoSpeedCodec());
      _cvResult = new Mock<ICvResultResponseHandler>();
      _cvNack = new Mock<ICvNackResponseHandler>();
      _cvNackSc = new Mock<ICvNackShortCircuitResponseHandler>();
      Z21ResponseHandler dispatcher = new(_transport, new Z21FrameReader(), new List<IZ21ResponseHandler>());

      _station = new Z21CommandStation(
                                       _transport,
                                       dispatcher,
                                       factory,
                                       new Z21Options(),
                                       Mock.Of<ILocoInfoResponseHandler>(),
                                       Mock.Of<ITurnoutInfoResponseHandler>(),
                                       Mock.Of<IExtAccessoryInfoResponseHandler>(),
                                       Mock.Of<ISystemStateDataChangedResponseHandler>(),
                                       Mock.Of<IFirmwareVersionResponseHandler>(),
                                       Mock.Of<IStatusChangedResponseHandler>(),
                                       Mock.Of<ITrackPowerOnResponseHandler>(),
                                       Mock.Of<ITrackPowerOffResponseHandler>(),
                                       _cvResult.Object,
                                       _cvNack.Object,
                                       _cvNackSc.Object,
                                       Mock.Of<IRmBusDataChangedResponseHandler>(),
                                       Mock.Of<IFastClockDataResponseHandler>());

      _transport.SetConnected(true);
    }

    [TearDown]
    public void TearDown() => _station.Dispose();

    private void RaiseResult(ushort cvAddress, byte value) =>
      _cvResult.Raise(h => h.OnCvResultReceived += null, new CvResultReceivedEventArgs(cvAddress, value));

    private void RaiseNack() => _cvNack.Raise(h => h.OnCvNackReceived += null, EventArgs.Empty);

    private void RaiseShortCircuit() => _cvNackSc.Raise(h => h.OnCvNackShortCircuitReceived += null, EventArgs.Empty);

    private async Task WaitForSentAsync(int count)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      while (_transport.Sent.Count < count)
      {
        if (stopwatch.Elapsed > TimeSpan.FromSeconds(2))
          throw new TimeoutException($"Expected {count} sent datagrams, saw {_transport.Sent.Count}.");
        await Task.Delay(5);
      }
    }

    [Test]
    public async Task ReadCvAsync_ResultReturnsValue()
    {
      Task<byte> task = _station.ReadCvAsync(5, TimeSpan.FromSeconds(2));
      RaiseResult(5, 42);

      byte value = await task;
      Assert.Multiple(() =>
      {
        Assert.That(value, Is.EqualTo(42));
        Assert.That(_transport.Sent, Has.Count.EqualTo(1));
      });
    }

    [Test]
    public async Task ReadCvAsync_RetriesOnNackThenReturnsValue()
    {
      Task<byte> task = _station.ReadCvAsync(5, TimeSpan.FromSeconds(5));

      RaiseNack();
      await WaitForSentAsync(2);
      RaiseResult(5, 99);

      byte value = await task;
      Assert.Multiple(() =>
      {
        Assert.That(value, Is.EqualTo(99));
        Assert.That(_transport.Sent, Has.Count.EqualTo(2));
      });
    }

    [Test]
    public void ReadCvAsync_ShortCircuitThrowsAndDoesNotRetry()
    {
      Task<byte> task = _station.ReadCvAsync(5, TimeSpan.FromSeconds(2));
      RaiseShortCircuit();

      Assert.ThrowsAsync<CvShortCircuitException>(async () => await task);
      Assert.That(_transport.Sent, Has.Count.EqualTo(1));
    }

    [Test]
    public void ReadCvAsync_NoResponseThrowsTimeout()
    {
      Task<byte> task = _station.ReadCvAsync(5, TimeSpan.FromMilliseconds(100));

      Assert.ThrowsAsync<CvOperationTimeoutException>(async () => await task);
    }

    [Test]
    public async Task WriteCvAsync_ResultCompletes()
    {
      Task task = _station.WriteCvAsync(7, 200, TimeSpan.FromSeconds(2));
      RaiseResult(7, 200);

      await task;
      Assert.That(_transport.Sent, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task WriteCvAsync_RetriesOnNackThenCompletes()
    {
      Task task = _station.WriteCvAsync(7, 200, TimeSpan.FromSeconds(5));

      RaiseNack();
      await WaitForSentAsync(2);
      RaiseResult(7, 200);

      await task;
      Assert.That(_transport.Sent, Has.Count.EqualTo(2));
    }

    [Test]
    public void WriteCvAsync_NoResponseThrowsTimeout()
    {
      Task task = _station.WriteCvAsync(7, 200, TimeSpan.FromMilliseconds(100));

      Assert.ThrowsAsync<CvOperationTimeoutException>(async () => await task);
    }

    [Test]
    public async Task ReadPomCvAsync_ResultReturnsValue()
    {
      Task<byte> task = _station.ReadPomCvAsync(3, 5, TimeSpan.FromSeconds(2));
      RaiseResult(5, 17);

      Assert.That(await task, Is.EqualTo(17));
    }

    [Test]
    public void ReadPomCvAsync_NoRailComReplyThrowsTimeout()
    {
      Task<byte> task = _station.ReadPomCvAsync(3, 5, TimeSpan.FromMilliseconds(100));

      Assert.ThrowsAsync<CvOperationTimeoutException>(async () => await task);
    }

    [Test]
    public async Task WritePomCvAsync_ReadBackMatchesCompletes()
    {
      Task task = _station.WritePomCvAsync(3, 5, 50, TimeSpan.FromSeconds(2));

      await WaitForSentAsync(2); // POM write + POM read-back
      RaiseResult(5, 50);

      await task;
      Assert.That(_transport.Sent, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task WritePomCvAsync_RetriesUntilReadBackMatches()
    {
      Task task = _station.WritePomCvAsync(3, 5, 50, TimeSpan.FromSeconds(5));

      await WaitForSentAsync(2); // write + read-back
      RaiseResult(5, 13);        // read-back mismatch -> rewrite + reread
      await WaitForSentAsync(4);
      RaiseResult(5, 50);        // matches -> done

      await task;
      Assert.That(_transport.Sent, Has.Count.EqualTo(4));
    }

    [Test]
    public void WritePomCvAsync_NoReplyThrowsTimeout()
    {
      Task task = _station.WritePomCvAsync(3, 5, 50, TimeSpan.FromMilliseconds(100));

      Assert.ThrowsAsync<CvOperationTimeoutException>(async () => await task);
    }
  }
}
