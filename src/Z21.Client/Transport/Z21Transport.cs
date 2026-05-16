using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Transport
{
  public class Z21Transport : IZ21Transport, IDisposable
  {
    private Lazy<UdpClient> _udpClient;

    public Z21Transport(Z21Configuration z21Configuration)
    {
      ArgumentNullException.ThrowIfNull(z21Configuration);
      Z21Configuration = z21Configuration;
      Z21Configuration.ConfigurationUpdated += (_, _) => _udpClient = new(UdpClientFactory());
      _udpClient = new(UdpClientFactory());
    }

    private UdpClient UdpClientFactory()
    {
      if (_udpClient?.IsValueCreated == true)
        _udpClient.Value.Dispose();

      var udpClient = new UdpClient(Z21Configuration.ClientIPEndPoint.Port);

      if (OperatingSystem.IsWindows())
        udpClient.AllowNatTraversal(Z21Configuration.AllowNatTraversal);
      return udpClient;
    }

    public event EventHandler<ResponseReceivedEventArgs>? OnResponseReceived;

    public bool IsConnected { get; private set; } = false;

    public Z21Configuration Z21Configuration { get; }

    public void Connect()
    {
      _udpClient.Value.Connect(Z21Configuration.ClientIPEndPoint);
      _udpClient.Value.BeginReceive(Receiving, null);
      IsConnected = true;
    }

    private void Receiving(IAsyncResult res)
    {
      IPEndPoint? remoteIpEndPoint = null!;
      byte[] received = _udpClient.Value.EndReceive(res, ref remoteIpEndPoint);
      _udpClient.Value.BeginReceive(Receiving, null);

      if (remoteIpEndPoint is not null
          && remoteIpEndPoint.Equals(Z21Configuration.ClientIPEndPoint))
        OnResponseReceived?.Invoke(this, new(received));
    }

    public async Task SendAsync(byte[] datagram)
    {
      ArgumentNullException.ThrowIfNull(datagram);
      await _udpClient.Value.SendAsync(datagram, datagram.GetLength(0));
    }

    public void Dispose()
    {
      if (_udpClient.IsValueCreated)
        _udpClient.Value.Dispose();
    }
  }
}