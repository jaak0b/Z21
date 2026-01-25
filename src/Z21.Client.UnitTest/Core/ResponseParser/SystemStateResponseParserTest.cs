using Moq;
using NUnit.Framework.Constraints;
using Z21.Core.Model;
using Z21.Core.ResponseParser;

namespace Z21.UnitTest.Core.ResponseParser
{
  public class SystemStateResponseParserTest
  {
    private SystemStateResponseParser _handler;
    private Mock<ICentralStateResponseParser> _centralStateResponseParserMock;
    private Mock<ICentralStateExResponseParser> _centralStateExResponseParserMock;
    private Mock<ICapabilitiesResponseParser> _capabilitiesResponseParserMock;

    [SetUp]
    public void Setup()
    {
      _centralStateResponseParserMock = new(MockBehavior.Strict);
      _centralStateExResponseParserMock = new(MockBehavior.Strict);
      _capabilitiesResponseParserMock = new(MockBehavior.Strict);
      _handler = new(_centralStateResponseParserMock.Object, _centralStateExResponseParserMock.Object, _capabilitiesResponseParserMock.Object);
    }

    [Test]
    public void Parse()
    {
      byte[] data = [0xFF, 0x7F, 0xFE, 0x7E, 0xFD, 0x7D, 0xFC, 0x7C, 0xF7, 0xF6, 0xF5, 0xF4, 0xF3, 0xF2, 0xF1, 0xF0];

      CentralState centralState = new();
      CentralStateEx centralStateEx = new();
      Capabilities capabilities = new();
      _centralStateResponseParserMock.Setup(parser => parser.Parse(0xF3))
                                     .Returns(centralState)
                                     .Verifiable(Times.Once);
      _centralStateExResponseParserMock.Setup(parser => parser.Parse(0xF2))
                                       .Returns(centralStateEx)
                                       .Verifiable(Times.Once);
      _capabilitiesResponseParserMock.Setup(parser => parser.Parse(0xF0))
                                     .Returns(capabilities)
                                     .Verifiable(Times.Once);

      SystemState value = _handler.Parse(data);
      Assert.Multiple(
                      () =>
                      {
                        Assert.That(value.VccVoltage, Is.EqualTo(62709));
                        Assert.That(value.Temperature, Is.EqualTo(31996));
                        Assert.That(value.SupplyVoltage, Is.EqualTo(63223));
                        Assert.That(value.ProgCurrent, Is.EqualTo(32510));
                        Assert.That(value.MainCurrent, Is.EqualTo(32767));
                        Assert.That(value.FilteredMainCurrent, Is.EqualTo(32253));
                        Assert.That(value.CentralState, Is.SameAs(centralState));
                        Assert.That(value.CentralStateEx, Is.SameAs(centralStateEx));
                        Assert.That(value.Capabilities, Is.SameAs(capabilities));
                      });
      Mock.VerifyAll();
    }
  }
}