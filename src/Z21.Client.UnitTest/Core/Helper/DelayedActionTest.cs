using System.Threading;
using Z21.Core.Helper;

namespace Z21.UnitTest.Core.Helper
{
  public class DelayedActionTest
  {
    [Test]
    public async Task Delay_FiresActionAfterInterval()
    {
      int count = 0;
      using DelayedAction action = new(TimeSpan.FromMilliseconds(40), () =>
                                                                       {
                                                                         Interlocked.Increment(ref count);
                                                                         return Task.CompletedTask;
                                                                       });

      action.Delay();
      await Task.Delay(150);

      Assert.That(Volatile.Read(ref count), Is.GreaterThanOrEqualTo(1));
    }

    [Test]
    public async Task Stop_PreventsFurtherFiring()
    {
      int count = 0;
      using DelayedAction action = new(TimeSpan.FromMilliseconds(40), () =>
                                                                       {
                                                                         Interlocked.Increment(ref count);
                                                                         return Task.CompletedTask;
                                                                       });

      action.Delay();
      await Task.Delay(150);
      action.Stop();
      int snapshot = Volatile.Read(ref count);
      await Task.Delay(150);

      Assert.Multiple(() =>
                      {
                        Assert.That(snapshot, Is.GreaterThanOrEqualTo(1), "timer should fire while running");
                        Assert.That(Volatile.Read(ref count), Is.EqualTo(snapshot), "no further firing after Stop");
                      });
    }
  }
}
