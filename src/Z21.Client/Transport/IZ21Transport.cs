using System;
using System.Threading.Tasks;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Transport
{
  public interface IZ21Transport
  {
    bool IsConnected { get; }
    event EventHandler<ResponseReceivedEventArgs>? OnResponseReceived;

    public Z21Configuration Z21Configuration { get; }

    void Connect();

    Task SendAsync(byte[] datagram);
  }
}