using Z21.Core.Model;
using Z21.Core.ResponseParser;

namespace Z21.UnitTest.Core.ResponseParser
{
  [TestFixture]
  public class CentralStateResponseParserTests
  {
    private CentralStateResponseParser _parser;

    [SetUp]
    public void Setup()
    {
      _parser = new();
    }

    [TestCase(0x00, false, false, false, false)]
    [TestCase(0x01, true, false, false, false)]
    [TestCase(0x02, false, true, false, false)]
    [TestCase(0x04, false, false, true, false)]
    [TestCase(0x20, false, false, false, true)]
    [TestCase(0x01 | 0x02 | 0x4, true, true, true, false)]
    [TestCase(0x01 | 0x20, true, false, false, true)]
    [TestCase(0x01 | 0x02 | 0x04 | 0x20, true, true, true, true)]
    public void Parse_ShouldSetCorrectFlags(byte statusByte, bool emergencyStop, bool trackVoltageOff, bool shortCircuit, bool programmingModeActive)
    {
      CentralState result = _parser.Parse(statusByte);

      Assert.Multiple(
                      () =>
                      {
                        Assert.That(result.EmergencyStop, Is.EqualTo(emergencyStop));
                        Assert.That(result.TrackVoltageOff, Is.EqualTo(trackVoltageOff));
                        Assert.That(result.ShortCircuit, Is.EqualTo(shortCircuit));
                        Assert.That(result.ProgrammingModeActive, Is.EqualTo(programmingModeActive));
                      });
    }
  }
}