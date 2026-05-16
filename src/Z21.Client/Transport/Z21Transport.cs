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
    public Z21Transport(Z21Configuration z21Configuration)
    {
      ArgumentNullException.ThrowIfNull(z21Configuration, nameof(z21Configuration));
      Z21Configuration = z21Configuration;

      UdpClient = new(Z21Configuration.ClientIPEndPoint.Port);

      if (OperatingSystem.IsWindows())
        UdpClient.AllowNatTraversal(Z21Configuration.AllowNatTraversal);
    }

    public bool IsConnected { get; private set; } = false;

    public event EventHandler<ResponseReceivedEventArgs>? OnResponseReceived;

    public UdpClient UdpClient { get; }

    public Z21Configuration Z21Configuration { get; }

    public void Connect()
    {
      UdpClient.Connect(Z21Configuration.ClientIPEndPoint);
      UdpClient.BeginReceive(Receiving, null);
      IsConnected = true;
    }

    private void Receiving(IAsyncResult res)
    {
      IPEndPoint? remoteIpEndPoint = null!;
      byte[] received = UdpClient.EndReceive(res, ref remoteIpEndPoint);
      UdpClient.BeginReceive(Receiving, null);

      if (remoteIpEndPoint is not null
          && remoteIpEndPoint.Equals(Z21Configuration.ClientIPEndPoint))
        OnResponseReceived?.Invoke(this, new(received));
    }

    public async Task SendAsync(byte[] datagram)
    {
      ArgumentNullException.ThrowIfNull(datagram, nameof(datagram));
      await UdpClient.SendAsync(datagram, datagram.GetLength(0));
    }

    public void Dispose()
    {
      UdpClient.Dispose();
    }
  }
}