using Z21.Core.Helper;

namespace Z21.UnitTest.Core.Helper
{
  public class AddressHelperTest
  {
    [Test]
    [TestCase((ushort)0, 0x00, 0x00)]
    [TestCase((ushort)1, 0x01, 0x00)]
    [TestCase((ushort)255, 0xFF, 0x00)]
    [TestCase((ushort)256, 0x00, 0x01)]
    [TestCase((ushort)512, 0x00, 0x02)]
    [TestCase((ushort)1023, 0xFF, 0x03)]
    [TestCase((ushort)1234, 0xD2, 0x04)]
    [TestCase((ushort)16383, 0xFF, 0x3F)]
    public void SplitLocoAddress_ReturnsCorrectLSBAndMSB(ushort input, byte expectedLsb, byte expectedMsb)
    {
      (byte lsb, byte msb) = AddressHelper.SplitLocoAddress(input);

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
      (byte lsb, byte msb) = AddressHelper.SplitAccessoryAddress(48);
      Assert.Multiple(() =>
                      {
                        Assert.That((msb << 8) + lsb, Is.EqualTo(47));
                        Assert.That(msb, Is.EqualTo(0x00));
                        Assert.That(lsb, Is.EqualTo(0x2F));
                      });
    }

    [Test]
    public void SplitAccessoryAddress_AddressIs0_ThrowsArgumentOutOfRangeException()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => AddressHelper.SplitAccessoryAddress(0));
    }

    [Test]
    public void CombineAccessoryAddress_ReturnsCorrectAddress()
    {
      const byte msb = 0x00;
      const byte lsb = 0x2f;
      ushort address = AddressHelper.CombineAccessoryAddress(lsb, msb);
      Assert.That(address, Is.EqualTo(48));
    }
  }
}