using System;
using CommandStation;
using Z21.Core;

namespace Z21.Client.UnitTest.Core
{
  public class IZ21CommandStationContractTest
  {
    [Test]
    public void IZ21CommandStation_ExposesEveryCapabilityTheStationImplements()
    {
      Type station = typeof(IZ21CommandStation);

      Assert.Multiple(() =>
      {
        Assert.That(typeof(ILocoControl).IsAssignableFrom(station), Is.True, "ILocoControl");
        Assert.That(typeof(IAccessoryControl).IsAssignableFrom(station), Is.True, "IAccessoryControl");
        Assert.That(typeof(ITrackPowerControl).IsAssignableFrom(station), Is.True, "ITrackPowerControl");
        Assert.That(typeof(ISystemInfoProvider).IsAssignableFrom(station), Is.True, "ISystemInfoProvider");
        Assert.That(typeof(IProgrammingControl).IsAssignableFrom(station), Is.True, "IProgrammingControl");
        Assert.That(typeof(IFeedbackControl).IsAssignableFrom(station), Is.True, "IFeedbackControl");
        Assert.That(typeof(IFastClockControl).IsAssignableFrom(station), Is.True, "IFastClockControl");
      });
    }
  }
}
