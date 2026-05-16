using System;
using System.Net.NetworkInformation;
using System.Timers;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;

namespace Z21.Core
{
  public sealed class Z21Watchdog
  {
    private readonly Z21Configuration _configuration;
    private readonly Timer _timer;
    private bool? _lastReachable;

    public event EventHandler<ConnectionChangedEventArgs>? OnReachabilityChanged;

    public Z21Watchdog(Z21Configuration configuration)
    {
      _configuration = configuration;

      _timer = new(TimeSpan.FromSeconds(1))
               {
                 AutoReset = true,
                 Enabled = true
               };
      _timer.Elapsed += (_, _) => CheckState();
    }

    private void CheckState()
    {
      var reachable = IsReachable();

      if (_lastReachable == reachable)
        return;

      _lastReachable = reachable;
      OnReachabilityChanged?.Invoke(this, new (reachable));
    }

    private bool IsReachable()
    {
      try
      {
        using var ping = new Ping();
        var reply = ping.Send(_configuration.ClientIPEndPoint.Address, 1000);
        return reply.Status == IPStatus.Success;
      }
      catch
      {
        return false;
      }
    }
  }
}