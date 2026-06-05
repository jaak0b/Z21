using Z21.Core.Framing;

namespace Z21.UnitTest.Core.Framing
{
  public class Z21FrameBuilderTest
  {
    private Z21FrameBuilder _builder = null!;

    [SetUp]
    public void SetUp() => _builder = new Z21FrameBuilder();

    [Test]
    public void BuildLan_NoPayload_WritesLengthAndHeaderOnly()
    {
      Assert.That(_builder.BuildLan(0x0010), Is.EqualTo(new byte[] { 0x04, 0x00, 0x10, 0x00 }));
    }

    [Test]
    public void BuildLan_WithPayload_AppendsPayloadAndSetsLength()
    {
      Assert.That(_builder.BuildLan(0x0050, 0x01, 0x02, 0x03, 0x04),
                  Is.EqualTo(new byte[] { 0x08, 0x00, 0x50, 0x00, 0x01, 0x02, 0x03, 0x04 }));
    }

    [Test]
    public void BuildLan_LowHeaderByteVaries_IsWrittenLittleEndian()
    {
      Assert.That(_builder.BuildLan(0x0061, 0xAA, 0xBB, 0x02),
                  Is.EqualTo(new byte[] { 0x07, 0x00, 0x61, 0x00, 0xAA, 0xBB, 0x02 }));
    }

    [Test]
    public void BuildXBus_AppendsXorOverXHeaderAndData()
    {
      Assert.That(_builder.BuildXBus(0xF1, 0x0A),
                  Is.EqualTo(new byte[] { 0x07, 0x00, 0x40, 0x00, 0xF1, 0x0A, 0xFB }));
    }

    [Test]
    public void BuildXBus_NoData_XorIsXHeaderItself()
    {
      Assert.That(_builder.BuildXBus(0x80),
                  Is.EqualTo(new byte[] { 0x06, 0x00, 0x40, 0x00, 0x80, 0x80 }));
    }

    [Test]
    public void BuildXBus_MatchesSetLocoDriveFrame()
    {
      Assert.That(_builder.BuildXBus(0xE4, 0x13, 0x00, 0x14, 0x82),
                  Is.EqualTo(new byte[] { 0x0A, 0x00, 0x40, 0x00, 0xE4, 0x13, 0x00, 0x14, 0x82, 0x61 }));
    }

    [Test]
    public void BuildLanChecksummed_AppendsXorOverDataBytes()
    {
      Assert.That(_builder.BuildLanChecksummed(0x00CC, 0x21, 0x2A),
                  Is.EqualTo(new byte[] { 0x07, 0x00, 0xCC, 0x00, 0x21, 0x2A, 0x0B }));
    }

    [Test]
    public void BuildLanChecksummed_NoData_AppendsZeroChecksum()
    {
      Assert.That(_builder.BuildLanChecksummed(0x00CC),
                  Is.EqualTo(new byte[] { 0x05, 0x00, 0xCC, 0x00, 0x00 }));
    }

    [Test]
    public void BuildLan_NullPayload_Throws()
    {
      Assert.Throws<System.ArgumentNullException>(() => _builder.BuildLan(0x0010, (byte[])null!));
    }

    [Test]
    public void BuildXBus_NullData_Throws()
    {
      Assert.Throws<System.ArgumentNullException>(() => _builder.BuildXBus(0x80, (byte[])null!));
    }

    [Test]
    public void BuildLanChecksummed_NullData_Throws()
    {
      Assert.Throws<System.ArgumentNullException>(() => _builder.BuildLanChecksummed(0x00CC, (byte[])null!));
    }

    [Test]
    public void BuildLan_PayloadOver255_WritesHighLengthByte()
    {
      byte[] payload = new byte[252]; // total length = 4 + 252 = 256 = 0x0100
      byte[] frame = _builder.BuildLan(0x0010, payload);

      Assert.Multiple(() =>
                      {
                        Assert.That(frame, Has.Length.EqualTo(256));
                        Assert.That(frame[0], Is.EqualTo(0x00), "low length byte");
                        Assert.That(frame[1], Is.EqualTo(0x01), "high length byte must be set");
                      });
    }
  }
}
