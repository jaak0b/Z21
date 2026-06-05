using System;
using System.Threading.Tasks;
using System.Timers;

namespace Z21.Core.Helper
{
  public class DelayedAction : IDisposable
  {
    private readonly Timer _connectionKeepAlive;

    public DelayedAction(TimeSpan delayTime, Func<Task> action)
    {
      _connectionKeepAlive = new(delayTime)
                             {
                               AutoReset = true,
                               Enabled = false
                             };

      _connectionKeepAlive.Elapsed += async (_, _) => await action();
    }

    public void Delay()
    {
      _connectionKeepAlive.Stop();
      _connectionKeepAlive.Start();
    }

    public void Stop() => _connectionKeepAlive.Stop();

    public void Dispose()
    {
      _connectionKeepAlive.Dispose();
      GC.SuppressFinalize(this);
    }
  }
}