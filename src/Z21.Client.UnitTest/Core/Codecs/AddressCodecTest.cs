using Z21.Core.Codecs;

namespace Z21.UnitTest.Core.Codecs
{
  public class AddressCodecTest
  {
    private AddressCodec _codec = null!;

    [SetUp]
    public void SetUp() => _codec = new AddressCodec();

    [Test]
    [TestCase((ushort)24, 0x00, 0x18)]
    [TestCase((ushort)0, 0x00, 0x00)]
    [TestCase((ushort)255, 0x00, 0xFF)]
    [TestCase((ushort)256, 0x01, 0x00)]
    [TestCase((ushort)300, 0x01, 0x2C)]
    public void SplitAddressBigEndian_ReturnsMsbThenLsb(ushort input, byte expectedMsb, byte expectedLsb)
    {
      (byte msb, byte lsb) = _codec.SplitAddressBigEndian(input);

      Assert.Multiple(() =>
                      {
                        Assert.That(msb, Is.EqualTo(expectedMsb));
                        Assert.That(lsb, Is.EqualTo(expectedLsb));
                      });
    }

    [Test]
    [TestCase((ushort)0, 0x00, 0x00)]
    [TestCase((ushort)1, 0x01, 0x00)]
    [TestCase((ushort)127, 0x7F, 0x00)]
    [TestCase((ushort)128, 0x80, 0x00)]
    [TestCase((ushort)255, 0xFF, 0x00)]
    [TestCase((ushort)256, 0x00, 0x01)]
    [TestCase((ushort)512, 0x00, 0x02)]
    [TestCase((ushort)1023, 0xFF, 0x03)]
    [TestCase((ushort)1234, 0xD2, 0x04)]
    [TestCase((ushort)16383, 0xFF, 0x3F)]
    public void SplitLocoAddress_ReturnsCorrectLSBAndMSB(ushort input, byte expectedLsb, byte expectedMsb)
    {
      (byte lsb, byte msb) = _codec.SplitLocoAddress(input);

      if (input >= 128)
        expectedMsb |= 0xC0;

      Assert.Multiple(() =>
                      {
                        Assert.That(lsb, Is.EqualTo(expectedLsb), "LSB is incorrect");
                        Assert.That(msb, Is.EqualTo(expectedMsb), "MSB is incorrect");
                      });
    }

    [Test]
    public void SplitAccessoryAddress_ReturnsCorrectLSBAndMSB()
    {
      (byte lsb, byte msb) = _codec.SplitAccessoryAddress(48);
      Assert.Multiple(() =>
                      {
                        Assert.That((msb << 8) + lsb, Is.EqualTo(47));
                        Assert.That(msb, Is.EqualTo(0x00));
                        Assert.That(lsb, Is.EqualTo(0x2F));
                      });
    }

    [Test]
    public void SplitAccessoryAddress_LargeAddress_FillsMsb()
    {
      (byte lsb, byte msb) = _codec.SplitAccessoryAddress(300);
      Assert.Multiple(() =>
                      {
                        Assert.That(msb, Is.EqualTo(0x01), "MSB must carry the high byte of (address - 1)");
                        Assert.That(lsb, Is.EqualTo(0x2B));
                      });
    }

    [Test]
    public void SplitAccessoryAddress_AddressIs1_DoesNotThrow()
    {
      Assert.DoesNotThrow(() => _codec.SplitAccessoryAddress(1));
    }

    [Test]
    public void SplitAccessoryAddress_AddressIs0_ThrowsWithMessage()
    {
      ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => _codec.SplitAccessoryAddress(0))!;
      Assert.That(exception.Message, Does.Contain("Smallest address is 1"));
    }

    [Test]
    public void SplitExtAccessoryAddress_AddressIs0_ThrowsWithMessage()
    {
      ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => _codec.SplitExtAccessoryAddress(0))!;
      Assert.That(exception.Message, Does.Contain("Smallest address is 1"));
    }

    [Test]
    public void CombineAccessoryAddress_ReturnsCorrectAddress()
    {
      const byte msb = 0x00;
      const byte lsb = 0x2f;
      ushort address = _codec.CombineAccessoryAddress(lsb, msb);
      Assert.That(address, Is.EqualTo(48));
    }

    [Test]
    public void CombineAccessoryAddress_WithMsb_ShiftsHighByte()
    {
      Assert.That(_codec.CombineAccessoryAddress(0x00, 0x01), Is.EqualTo(257));
    }

    [Test]
    public void CombineExtAccessoryAddress_WithMsb_ShiftsHighByte()
    {
      Assert.That(_codec.CombineExtAccessoryAddress(0x00, 0x01), Is.EqualTo(253));
    }

    [Test]
    [TestCase((ushort)1, 0x04, 0x00)]
    [TestCase((ushort)2, 0x05, 0x00)]
    [TestCase((ushort)253, 0x00, 0x01)]
    public void SplitExtAccessoryAddress_MapsUserAddressToRawAddress(ushort userAddress, byte expectedLsb, byte expectedMsb)
    {
      (byte lsb, byte msb) = _codec.SplitExtAccessoryAddress(userAddress);
      Assert.Multiple(() =>
                      {
                        Assert.That(lsb, Is.EqualTo(expectedLsb), "LSB is incorrect");
                        Assert.That(msb, Is.EqualTo(expectedMsb), "MSB is incorrect");
                      });
    }

    [Test]
    public void SplitExtAccessoryAddress_AddressIs0_ThrowsArgumentOutOfRangeException()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => _codec.SplitExtAccessoryAddress(0));
    }

    [Test]
    public void CombineExtAccessoryAddress_IsInverseOfSplit()
    {
      (byte lsb, byte msb) = _codec.SplitExtAccessoryAddress(1);
      Assert.Multiple(() =>
                      {
                        Assert.That((msb << 8) + lsb, Is.EqualTo(4), "RawAddress for user address 1 must be 4");
                        Assert.That(_codec.CombineExtAccessoryAddress(lsb, msb), Is.EqualTo((ushort)1));
                      });
    }

    [Test]
    [TestCase((ushort)0, 0x00, 0x00)]
    [TestCase((ushort)1, 0x00, 0x01)]
    [TestCase((ushort)255, 0x00, 0xFF)]
    [TestCase((ushort)256, 0x01, 0x00)]
    [TestCase((ushort)1021, 0x03, 0xFD)]
    public void SplitCvAddress_ReturnsCorrectMsbAndLsb(ushort cvAddress, byte expectedMsb, byte expectedLsb)
    {
      (byte msb, byte lsb) = _codec.SplitCvAddress(cvAddress);
      Assert.Multiple(() =>
                      {
                        Assert.That(msb, Is.EqualTo(expectedMsb), "MSB is incorrect");
                        Assert.That(lsb, Is.EqualTo(expectedLsb), "LSB is incorrect");
                      });
    }

    [Test]
    [TestCase((ushort)0)]
    [TestCase((ushort)255)]
    [TestCase((ushort)1021)]
    public void CombineCvAddress_IsInverseOfSplit(ushort cvAddress)
    {
      (byte msb, byte lsb) = _codec.SplitCvAddress(cvAddress);
      Assert.That(_codec.CombineCvAddress(msb, lsb), Is.EqualTo(cvAddress));
    }

    [Test]
    [TestCase((ushort)0, 0x00, 0x00)]
    [TestCase((ushort)1, 0x00, 0x01)]
    [TestCase((ushort)255, 0x00, 0xFF)]
    [TestCase((ushort)256, 0x01, 0x00)]
    [TestCase((ushort)768, 0x03, 0x00)]
    [TestCase((ushort)1023, 0x03, 0xFF)]
    public void SplitPomCvAddress_ReturnsCvHighBitsAndLsb(ushort cvAddress, byte expectedHighBits, byte expectedLsb)
    {
      (byte cvHighBits, byte cvLsb) = _codec.SplitPomCvAddress(cvAddress);
      Assert.Multiple(() =>
                      {
                        Assert.That(cvHighBits, Is.EqualTo(expectedHighBits), "CV high bits are incorrect");
                        Assert.That(cvLsb, Is.EqualTo(expectedLsb), "CV LSB is incorrect");
                      });
    }

    [Test]
    [TestCase((ushort)1024)]
    [TestCase((ushort)1025)]
    [TestCase((ushort)65535)]
    public void SplitPomCvAddress_AboveTenBitRange_ThrowsWithMessage(ushort cvAddress)
    {
      ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => _codec.SplitPomCvAddress(cvAddress))!;
      Assert.That(exception.Message, Does.Contain("0 and 1023"));
    }

    [Test]
    public void EncodeAccessoryPomAddress_WholeDecoder_SetsCddNibbleToZero()
    {
      (byte db1, byte db2) = _codec.EncodeAccessoryPomAddress(1, wholeDecoder: true, output: 0);
      Assert.Multiple(() =>
                      {
                        Assert.That(db1, Is.EqualTo(0x00), "DB1 (aaaaa) is incorrect");
                        Assert.That(db2, Is.EqualTo(0x10), "DB2 (AAAACDDD) is incorrect");
                      });
    }

    [Test]
    public void EncodeAccessoryPomAddress_SingleOutput_SetsCbitAndOutput()
    {
      (byte db1, byte db2) = _codec.EncodeAccessoryPomAddress(1, wholeDecoder: false, output: 3);
      Assert.Multiple(() =>
                      {
                        Assert.That(db1, Is.EqualTo(0x00), "DB1 (aaaaa) is incorrect");
                        Assert.That(db2, Is.EqualTo(0x1B), "DB2 (AAAACDDD) is incorrect");
                      });
    }

    [Test]
    public void EncodeAccessoryPomAddress_LargeAddress_FillsHighByte()
    {
      (byte db1, byte db2) = _codec.EncodeAccessoryPomAddress(0x1FF, wholeDecoder: true, output: 0);
      Assert.Multiple(() =>
                      {
                        Assert.That(db1, Is.EqualTo(0x1F), "DB1 (aaaaa) is incorrect");
                        Assert.That(db2, Is.EqualTo(0xF0), "DB2 (AAAACDDD) is incorrect");
                      });
    }
  }
}
