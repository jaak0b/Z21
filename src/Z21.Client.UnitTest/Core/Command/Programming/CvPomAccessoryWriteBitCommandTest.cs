using Z21.Core.Command.Programming;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Programming
{
  public class CvPomAccessoryWriteBitCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)1, true, (byte)0, (ushort)0, (byte)2, true, new byte[] { 0x0C, 0x00, 0x40, 0x00, 0xE6, 0x31, 0x00, 0x10, 0xE8, 0x00, 0x0A, 0x25 })]
    [TestCase((ushort)1, true, (byte)0, (ushort)0, (byte)2, false, new byte[] { 0x0C, 0x00, 0x40, 0x00, 0xE6, 0x31, 0x00, 0x10, 0xE8, 0x00, 0x02, 0x2D })]
    [TestCase((ushort)1, true, (byte)0, (ushort)256, (byte)2, true, new byte[] { 0x0C, 0x00, 0x40, 0x00, 0xE6, 0x31, 0x00, 0x10, 0xE9, 0x00, 0x0A, 0x24 })]
    public void Ctor_SetsCorrectDataBits(ushort decoderAddress, bool wholeDecoder, byte output, ushort cvAddress, byte bitPosition, bool bitValue, byte[] expected)
    {
      CvPomAccessoryWriteBitCommand command = Factory.Create<CvPomAccessoryWriteBitCommand>(decoderAddress, wholeDecoder, output, cvAddress, bitPosition, bitValue);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
