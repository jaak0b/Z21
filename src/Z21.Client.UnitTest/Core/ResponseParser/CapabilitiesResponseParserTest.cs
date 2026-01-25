using Z21.Core.Model;
using Z21.Core.ResponseParser;

namespace Z21.UnitTest.Core.ResponseParser
{
  public class CapabilitiesResponseParserTests
  {
    private CapabilitiesResponseParser _parser;

    [SetUp]
    public void Setup()
    {
      _parser = new();
    }

    [Test]
    public void Parse_StatusByteIsZero_ReturnsNull()
    {
      Capabilities? result = _parser.Parse(0x00);
      Assert.That(result, Is.Null);
    }

    [TestCase(0x01, true, false, false, false, false, false, false)]
    [TestCase(0x02, false, true, false, false, false, false, false)]
    [TestCase(0x08, false, false, true, false, false, false, false)]
    [TestCase(0x10, false, false, false, true, false, false, false)]
    [TestCase(0x20, false, false, false, false, true, false, false)]
    [TestCase(0x40, false, false, false, false, false, true, false)]
    [TestCase(0x80, false, false, false, false, false, false, true)]
    [TestCase(0x01 | 0x02 | 0x08 | 0x10 | 0x20 | 0x40 | 0x80, true, true, true, true, true, true, true)]
    public void Parse_IndividualBits_ReturnsCorrectCapabilities(byte statusByte, bool expectedDcc, bool expectedMm, bool expectedRailCom, bool expectedLocoCmds, bool expectedAccessoryCmds,
                                                                bool expectedDetectorCmds, bool expectedNeedsUnlockCode)
    {
      Capabilities? result = _parser.Parse(statusByte);

      Assert.That(result, Is.Not.Null);
      Assert.Multiple(
                      () =>
                      {
                        Assert.That(result.Dcc, Is.EqualTo(expectedDcc));
                        Assert.That(result.Mm, Is.EqualTo(expectedMm));
                        Assert.That(result.RailCom, Is.EqualTo(expectedRailCom));
                        Assert.That(result.LocoCmds, Is.EqualTo(expectedLocoCmds));
                        Assert.That(result.AccessoryCmds, Is.EqualTo(expectedAccessoryCmds));
                        Assert.That(result.DetectorCmds, Is.EqualTo(expectedDetectorCmds));
                        Assert.That(result.NeedsUnlockCode, Is.EqualTo(expectedNeedsUnlockCode));
                      });
    }
  }
}