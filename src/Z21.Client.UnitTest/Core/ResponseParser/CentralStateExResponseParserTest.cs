using Z21.Core.Model;
using Z21.Core.ResponseParser;

namespace Z21.UnitTest.Core.ResponseParser
{
  public class CentralStateExResponseParserTests
  {
    private CentralStateExResponseParser _parser;

    [SetUp]
    public void Setup()
    {
      _parser = new();
    }

    [TestCase(0x01, true, false, false, false, false)]
    [TestCase(0x02, false, true, false, false, false)]
    [TestCase(0x04, false, false, true, false, false)]
    [TestCase(0x08, false, false, false, true, false)]
    [TestCase(0x20, false, false, false, false, true)]
    [TestCase(0x01 | 0x02 | 0x04 | 0x08 | 0x20, true, true, true, true, true)]
    [TestCase(0x00, false, false, false, false, false)]
    public void Parse_ShouldSetCorrectFlags(byte statusByte, bool expectedHighTemp, bool expectedPowerLost, bool expectedShortExternal, bool expectedShortInternal, bool expectedRcn213)
    {
      CentralStateEx result = _parser.Parse(statusByte);

      Assert.Multiple(
                      () =>
                      {
                        Assert.That(result.HighTemperature, Is.EqualTo(expectedHighTemp));
                        Assert.That(result.PowerLost, Is.EqualTo(expectedPowerLost));
                        Assert.That(result.ShortCircuitExternal, Is.EqualTo(expectedShortExternal));
                        Assert.That(result.ShortCircuitInternal, Is.EqualTo(expectedShortInternal));
                        Assert.That(result.Rcn213, Is.EqualTo(expectedRcn213));
                      });
    }
  }
}