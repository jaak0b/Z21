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
    [TestCase(DccSpeedMode.Steps14, (ushort)15, "Steps14", "14")]
    [TestCase(DccSpeedMode.Steps28, (ushort)29, "Steps28", "28")]
    [TestCase(DccSpeedMode.Steps128, (ushort)127, "Steps128", "126")]
    public void ThrowIfOutOfRange_ValuesOutOfRange_ThrowsWithDescriptiveMessage(DccSpeedMode dccSpeedMode, ushort locoSpeed, string expectedFragment, string expectedMax)
    {
      LocoSpeedOutOfRangeException exception = Assert.Throws<LocoSpeedOutOfRangeException>(() => LocoSpeedOutOfRangeException.ThrowIfOutOfRange(dccSpeedMode, locoSpeed))!;
      Assert.Multiple(() =>
                      {
                        Assert.That(exception.Message, Does.Contain(expectedFragment));
                        Assert.That(exception.Message, Does.Contain($"maximum speed of {expectedMax} steps"),
                                    "the message must state the actual maximum that the guard enforces");
                      });
    }
  }
}