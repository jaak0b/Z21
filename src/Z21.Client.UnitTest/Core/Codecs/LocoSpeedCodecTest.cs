using Z21.Core.Codecs;
using Z21.Core.Model;

namespace Z21.UnitTest.Core.Codecs
{
  public class LocoSpeedCodecTest
  {
    private LocoSpeedCodec _codec = null!;

    [SetUp]
    public void SetUp() => _codec = new LocoSpeedCodec();

    [Test]
    [TestCase((ushort)0, (ushort)0)]
    [TestCase((ushort)1, (ushort)2)]
    [TestCase((ushort)2, (ushort)3)]
    [TestCase((ushort)13, (ushort)14)]
    public void CalculateDccSpeed_Dcc14(ushort speedStep, ushort dccSpeed)
    {
      Assert.That(_codec.CalculateDccSpeed(DccSpeedMode.Steps14, speedStep), Is.EqualTo(dccSpeed));
    }

    [Test]
    [TestCase((ushort)0, (ushort)0)]
    [TestCase((ushort)1, (ushort)2)]
    [TestCase((ushort)2, (ushort)3)]
    [TestCase((ushort)127, (ushort)128)]
    public void CalculateDccSpeed_Dcc128(ushort speedStep, ushort dccSpeed)
    {
      Assert.That(_codec.CalculateDccSpeed(DccSpeedMode.Steps128, speedStep), Is.EqualTo(dccSpeed));
    }

    [Test]
    [TestCase((ushort)0, (ushort)0)]
    [TestCase((ushort)1, (ushort)2)]
    [TestCase((ushort)2, (ushort)18)]
    [TestCase((ushort)3, (ushort)3)]
    [TestCase((ushort)4, (ushort)19)]
    [TestCase((ushort)5, (ushort)4)]
    [TestCase((ushort)6, (ushort)20)]
    [TestCase((ushort)7, (ushort)5)]
    [TestCase((ushort)8, (ushort)21)]
    [TestCase((ushort)9, (ushort)6)]
    [TestCase((ushort)10, (ushort)22)]
    [TestCase((ushort)11, (ushort)7)]
    [TestCase((ushort)12, (ushort)23)]
    [TestCase((ushort)13, (ushort)8)]
    [TestCase((ushort)14, (ushort)24)]
    [TestCase((ushort)15, (ushort)9)]
    [TestCase((ushort)16, (ushort)25)]
    [TestCase((ushort)17, (ushort)10)]
    [TestCase((ushort)18, (ushort)26)]
    [TestCase((ushort)19, (ushort)11)]
    [TestCase((ushort)20, (ushort)27)]
    [TestCase((ushort)21, (ushort)12)]
    [TestCase((ushort)22, (ushort)28)]
    [TestCase((ushort)23, (ushort)13)]
    [TestCase((ushort)24, (ushort)29)]
    [TestCase((ushort)25, (ushort)14)]
    [TestCase((ushort)26, (ushort)30)]
    [TestCase((ushort)27, (ushort)15)]
    [TestCase((ushort)28, (ushort)31)]
    public void CalculateDccSpeed_Dcc28(ushort speedStep, ushort dccSpeed)
    {
      Assert.That(_codec.CalculateDccSpeed(DccSpeedMode.Steps28, speedStep), Is.EqualTo(dccSpeed));
    }

    [TestCase((ushort)0, (ushort)0)]
    [TestCase((ushort)1, (ushort)0)]
    [TestCase((ushort)2, (ushort)1)]
    [TestCase((ushort)13, (ushort)12)]
    public void CalculateSpeedStep_Dcc14(ushort dccSpeed, ushort speedStep)
    {
      Assert.That(_codec.CalculateSpeedStep(DccSpeedMode.Steps14, dccSpeed), Is.EqualTo(speedStep));
    }

    [TestCase((ushort)0, (ushort)0)]
    [TestCase((ushort)1, (ushort)0)]
    [TestCase((ushort)2, (ushort)1)]
    [TestCase((ushort)129, (ushort)128)]
    public void CalculateSpeedStep_Dcc128(ushort dccSpeed, ushort speedStep)
    {
      Assert.That(_codec.CalculateSpeedStep(DccSpeedMode.Steps128, dccSpeed), Is.EqualTo(speedStep));
    }

    [Test]
    [TestCase(0, 0)]
    [TestCase(16, 0)]
    [TestCase(1, 0)]
    [TestCase(17, 0)]
    [TestCase(2, 1)]
    [TestCase(18, 2)]
    [TestCase(3, 3)]
    [TestCase(19, 4)]
    [TestCase(4, 5)]
    [TestCase(20, 6)]
    [TestCase(5, 7)]
    [TestCase(21, 8)]
    [TestCase(6, 9)]
    [TestCase(22, 10)]
    [TestCase(7, 11)]
    [TestCase(23, 12)]
    [TestCase(8, 13)]
    [TestCase(24, 14)]
    [TestCase(9, 15)]
    [TestCase(25, 16)]
    [TestCase(10, 17)]
    [TestCase(26, 18)]
    [TestCase(11, 19)]
    [TestCase(27, 20)]
    [TestCase(12, 21)]
    [TestCase(28, 22)]
    [TestCase(13, 23)]
    [TestCase(29, 24)]
    [TestCase(14, 25)]
    [TestCase(30, 26)]
    [TestCase(15, 27)]
    [TestCase(31, 28)]
    public void CalculateSpeedStep_Dcc28(short dccSpeed, short speedStep)
    {
      Assert.That(_codec.CalculateSpeedStep(DccSpeedMode.Steps28, (ushort)dccSpeed), Is.EqualTo((ushort)speedStep), $"Dcc Speed: {dccSpeed}. Expected speed step: {speedStep}");
    }

    [Test]
    [TestCase((ushort)32)]
    [TestCase((ushort)37)]
    [TestCase((ushort)127)]
    public void CalculateSpeedStep_Dcc28_OutOfRangeValue_ReturnsZeroAndDoesNotThrow(ushort dccSpeed)
    {
      Assert.That(() => _codec.CalculateSpeedStep(DccSpeedMode.Steps28, dccSpeed), Throws.Nothing);
      Assert.That(_codec.CalculateSpeedStep(DccSpeedMode.Steps28, dccSpeed), Is.EqualTo((ushort)0));
    }
  }
}
