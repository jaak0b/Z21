using Z21.Core.Exception;
using Z21.Core.Model;

namespace Z21.UnitTest.Core.Exception
{
  public class LocoSpeedOutOfRangeExceptionTest
  {
    [Test]
    [TestCase(DccSpeedMode.Steps14, (ushort)14)]
    [TestCase(DccSpeedMode.Steps28, (ushort)28)]
    [TestCase(DccSpeedMode.Steps128, (ushort)126)]
    public void ThrowIfOutOfRange_ValuesInRange_DoesNothing(DccSpeedMode dccSpeedMode, ushort locoSpeed)
    {
      Assert.DoesNotThrow(() => LocoSpeedOutOfRangeException.ThrowIfOutOfRange(dccSpeedMode, locoSpeed));
    }

    [Test]
    [TestCase(DccSpeedMode.Steps14, (ushort)15)]
    [TestCase(DccSpeedMode.Steps28, (ushort)29)]
    [TestCase(DccSpeedMode.Steps128, (ushort)127)]
    public void ThrowIfOutOfRange_ValuesOutOfRange_ThrowsLocoSpeedOutOfRangeException(DccSpeedMode dccSpeedMode, ushort locoSpeed)
    {
      Assert.Throws<LocoSpeedOutOfRangeException>(() => LocoSpeedOutOfRangeException.ThrowIfOutOfRange(dccSpeedMode, locoSpeed));
    }
  }
}