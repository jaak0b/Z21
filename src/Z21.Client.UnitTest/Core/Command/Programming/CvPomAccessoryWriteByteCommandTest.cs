using Z21.Core.Command.Programming;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Programming
{
  public class CvPomAccessoryWriteByteCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)1, true, (byte)0, (ushort)0, (byte)0x05, new byte[] { 0x0C, 0x00, 0x40, 0x00, 0xE6, 0x31, 0x00, 0x10, 0xEC, 0x00, 0x05, 0x2E })]
    public void Ctor_SetsCorrectDataBits(ushort decoderAddress, bool wholeDecoder, byte output, ushort cvAddress, byte value, byte[] expected)
    {
      CvPomAccessoryWriteByteCommand command = Factory.Create<CvPomAccessoryWriteByteCommand>(decoderAddress, wholeDecoder, output, cvAddress, value);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
