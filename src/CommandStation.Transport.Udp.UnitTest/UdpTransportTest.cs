using System.Net;
using System.Net.Sockets;
using CommandStation.Transport;

namespace CommandStation.Transport.Udp.UnitTest
{
  [TestFixture]
  public class UdpTransportTest
  {
    private UdpClient _station = null!;
    private IPEndPoint _stationEndPoint = null!;

    [SetUp]
    public void SetUp()
    {
      _station = new UdpClient(new IPEndPoint(IPAddress.Loopback, 0));
      _stationEndPoint = (IPEndPoint)_station.Client.LocalEndPoint!;
    }

    [TearDown]
    public void TearDown()
    {
      _station.Dispose();
    }

    [Test]
    public async Task ConnectAsync_SetsIsConnected_AndRaisesOnConnectionChanged()
    {
      await using var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = _stationEndPoint });
      bool? raised = null;
      transport.OnConnectionChanged += (_, args) => raised = args.IsConnected;

      await transport.ConnectAsync();

      Assert.That(transport.IsConnected, Is.True);
      Assert.That(raised, Is.True);
    }

    [Test]
    public async Task SendAsync_TransmitsBytes_ToRemoteEndpoint()
    {
      await using var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = _stationEndPoint });
      await transport.ConnectAsync();
      byte[] payload = [0x07, 0x00, 0x40, 0x00, 0x21, 0x21, 0x00];

      await transport.SendAsync(payload);

      var received = await _station.ReceiveAsync().WaitAsync(TimeSpan.FromSeconds(2));
      Assert.That(received.Buffer, Is.EqualTo(payload));
    }

    [Test]
    public async Task ReceiveLoop_OnSocketError_RaisesDisconnectedExactlyOnce()
    {
      int deadPort;
      using (UdpClient dead = new(new IPEndPoint(IPAddress.Loopback, 0)))
        deadPort = ((IPEndPoint)dead.Client.LocalEndPoint!).Port;

      var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = new IPEndPoint(IPAddress.Loopback, deadPort) });
      int disconnectedRaises = 0;
      TaskCompletionSource<bool> disconnected = new();
      transport.OnConnectionChanged += (_, args) =>
                                       {
                                         if (!args.IsConnected)
                                         {
                                           System.Threading.Interlocked.Increment(ref disconnectedRaises);
                                           disconnected.TrySetResult(true);
                                         }
                                       };
      try
      {
        await transport.ConnectAsync();

        await transport.SendAsync(new byte[] { 0x01 });

        Assert.That(await disconnected.Task.WaitAsync(TimeSpan.FromSeconds(5)), Is.True);
        Assert.Multiple(() =>
                        {
                          Assert.That(transport.IsConnected, Is.False);
                          Assert.That(disconnectedRaises, Is.EqualTo(1), "the lost connection must raise disconnected exactly once");
                        });
      }
      finally
      {
        await transport.DisposeAsync();
      }
    }

    [Test]
    public void Ctor_NullOptions_Throws()
    {
      Assert.Throws<System.ArgumentNullException>(() => _ = new UdpTransport(null!));
    }

    [Test]
    public void SendAsync_WhenNotConnected_ThrowsWithMessage()
    {
      var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = _stationEndPoint });
      var exception = Assert.ThrowsAsync<System.InvalidOperationException>(async () => await transport.SendAsync(new byte[] { 0x01 }))!;
      Assert.That(exception.Message, Does.Contain("not connected"));
    }

    [Test]
    public async Task ConnectAsync_WhenAlreadyConnected_IsIdempotent()
    {
      await using var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = _stationEndPoint });
      int connectedRaises = 0;
      transport.OnConnectionChanged += (_, args) =>
                                       {
                                         if (args.IsConnected)
                                           connectedRaises++;
                                       };

      await transport.ConnectAsync();
      await transport.ConnectAsync();

      Assert.Multiple(() =>
                      {
                        Assert.That(transport.IsConnected, Is.True);
                        Assert.That(connectedRaises, Is.EqualTo(1), "second connect must be a no-op");
                      });
    }

    [Test]
    public async Task DisconnectAsync_SetsDisconnected_AndRaisesOnce()
    {
      await using var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = _stationEndPoint });
      int disconnectedRaises = 0;
      transport.OnConnectionChanged += (_, args) =>
                                       {
                                         if (!args.IsConnected)
                                           disconnectedRaises++;
                                       };
      await transport.ConnectAsync();

      await transport.DisconnectAsync();
      await transport.DisconnectAsync();

      Assert.Multiple(() =>
                      {
                        Assert.That(transport.IsConnected, Is.False);
                        Assert.That(disconnectedRaises, Is.EqualTo(1), "second disconnect must be a no-op");
                      });
    }

    [Test]
    public async Task DisconnectAsync_WhenNeverConnected_DoesNotRaise()
    {
      await using var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = _stationEndPoint });
      bool raised = false;
      transport.OnConnectionChanged += (_, _) => raised = true;

      await transport.DisconnectAsync();

      Assert.Multiple(() =>
                      {
                        Assert.That(raised, Is.False);
                        Assert.That(transport.IsConnected, Is.False);
                      });
    }

    [Test]
    public async Task DisposeAsync_DisconnectsActiveTransport()
    {
      var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = _stationEndPoint });
      bool disconnected = false;
      transport.OnConnectionChanged += (_, args) =>
                                       {
                                         if (!args.IsConnected)
                                           disconnected = true;
                                       };
      await transport.ConnectAsync();

      await transport.DisposeAsync();

      Assert.Multiple(() =>
                      {
                        Assert.That(transport.IsConnected, Is.False);
                        Assert.That(disconnected, Is.True);
                      });
    }

    [Test]
    public async Task Dispose_DisconnectsActiveTransport_AndRaisesOnce()
    {
      var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = _stationEndPoint });
      int disconnectedRaises = 0;
      transport.OnConnectionChanged += (_, args) =>
                                       {
                                         if (!args.IsConnected)
                                           disconnectedRaises++;
                                       };
      await transport.ConnectAsync();

      transport.Dispose();

      Assert.Multiple(() =>
                      {
                        Assert.That(transport.IsConnected, Is.False);
                        Assert.That(disconnectedRaises, Is.EqualTo(1));
                      });
    }

    [Test]
    public void Dispose_WhenNeverConnected_DoesNotRaise()
    {
      var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = _stationEndPoint });
      bool raised = false;
      transport.OnConnectionChanged += (_, _) => raised = true;

      transport.Dispose();

      Assert.Multiple(() =>
                      {
                        Assert.That(raised, Is.False);
                        Assert.That(transport.IsConnected, Is.False);
                      });
    }

    [Test]
    public async Task Dispose_CalledTwice_RaisesDisconnectedOnce()
    {
      var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = _stationEndPoint });
      int disconnectedRaises = 0;
      transport.OnConnectionChanged += (_, args) =>
                                       {
                                         if (!args.IsConnected)
                                           disconnectedRaises++;
                                       };
      await transport.ConnectAsync();

      transport.Dispose();
      transport.Dispose();

      Assert.That(disconnectedRaises, Is.EqualTo(1), "second dispose must be a no-op");
    }

    [Test]
    public async Task IncomingBytes_RaiseOnBytesReceived()
    {
      await using var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = _stationEndPoint });
      var tcs = new TaskCompletionSource<byte[]>();
      transport.OnBytesReceived += (_, args) => tcs.TrySetResult(args.Data);
      await transport.ConnectAsync();

      // Provoke the station to learn the transport's source endpoint, then reply to it.
      await transport.SendAsync(new byte[] { 0x01 });
      var probe = await _station.ReceiveAsync().WaitAsync(TimeSpan.FromSeconds(2));
      byte[] reply = [0x07, 0x00, 0x40, 0x00, 0x61, 0x01, 0x60];
      await _station.SendAsync(reply, reply.Length, probe.RemoteEndPoint);

      byte[] got = await tcs.Task.WaitAsync(TimeSpan.FromSeconds(2));
      Assert.That(got, Is.EqualTo(reply));
    }

    [Test]
    public async Task ReceiveLoop_SubscriberThrows_LoopSurvivesAndKeepsDelivering()
    {
      await using var transport = new UdpTransport(new UdpTransportOptions { RemoteEndPoint = _stationEndPoint });
      int received = 0;
      var secondReceived = new TaskCompletionSource<bool>();
      transport.OnBytesReceived += (_, _) =>
                                   {
                                     if (System.Threading.Interlocked.Increment(ref received) == 1)
                                       throw new System.InvalidOperationException("boom in subscriber");
                                     secondReceived.TrySetResult(true);
                                   };
      await transport.ConnectAsync();

      await transport.SendAsync(new byte[] { 0x01 });
      var probe = await _station.ReceiveAsync().WaitAsync(TimeSpan.FromSeconds(2));
      byte[] reply = [0x07, 0x00, 0x40, 0x00, 0x61, 0x01, 0x60];
      await _station.SendAsync(reply, reply.Length, probe.RemoteEndPoint);
      await _station.SendAsync(reply, reply.Length, probe.RemoteEndPoint);

      Assert.Multiple(() =>
                      {
                        Assert.That(secondReceived.Task.WaitAsync(TimeSpan.FromSeconds(2)).Result, Is.True,
                                    "a throwing OnBytesReceived subscriber must not kill the receive loop");
                        Assert.That(transport.IsConnected, Is.True);
                      });
    }
  }
}
