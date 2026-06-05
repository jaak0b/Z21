using Z21.Core.Command.Programming;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Programming
{
  public class CvPomAccessoryReadByteCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)1, true, (byte)0, (ushort)0, new byte[] { 0x0C, 0x00, 0x40, 0x00, 0xE6, 0x31, 0x00, 0x10, 0xE4, 0x00, 0x00, 0x23 })]
    [TestCase((ushort)1, true, (byte)0, (ushort)256, new byte[] { 0x0C, 0x00, 0x40, 0x00, 0xE6, 0x31, 0x00, 0x10, 0xE5, 0x00, 0x00, 0x22 })]
    public void Ctor_SetsCorrectDataBits(ushort decoderAddress, bool wholeDecoder, byte output, ushort cvAddress, byte[] expected)
    {
      CvPomAccessoryReadByteCommand command = Factory.Create<CvPomAccessoryReadByteCommand>(decoderAddress, wholeDecoder, output, cvAddress);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
