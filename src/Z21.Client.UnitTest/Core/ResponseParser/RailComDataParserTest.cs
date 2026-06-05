using Z21.Core.Model;
using Z21.Core.ResponseParser;

namespace Z21.UnitTest.Core.ResponseParser
{
  public class RailComDataParserTest
  {
    private RailComDataParser _parser = null!;

    [SetUp]
    public void SetUp() => _parser = new();

    [Test]
    public void Parse_ReadsAllFields()
    {
      byte[] data = [0x03, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x05, 0x50, 0x0A, 0x00];

      RailComData result = _parser.Parse(data);

      Assert.Multiple(() =>
                      {
                        Assert.That(result.LocoAddress, Is.EqualTo(3));
                        Assert.That(result.ReceiveCounter, Is.EqualTo(255u));
                        Assert.That(result.ErrorCounter, Is.EqualTo(2));
                        Assert.That(result.Options, Is.EqualTo(RailComOptions.Speed1 | RailComOptions.QoS));
                        Assert.That(result.Speed, Is.EqualTo(80));
                        Assert.That(result.QualityOfService, Is.EqualTo(10));
                      });
    }
  }
}
