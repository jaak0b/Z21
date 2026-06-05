using Z21.Core.Command.LocoNet;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.LocoNet
{
  public class LocoNetCommandTest : CommandTestFixture
  {
    [Test]
    public void LocoNetFromLan_WrapsRawMessage()
    {
      LocoNetFromLanCommand command = Factory.Create<LocoNetFromLanCommand>(new byte[] { 0xB0, 0x01, 0x02, 0x03 });
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x08, 0x00, 0xA2, 0x00, 0xB0, 0x01, 0x02, 0x03 }));
    }

    [Test]
    [TestCase((ushort)3, new byte[] { 0x06, 0x00, 0xA3, 0x00, 0x03, 0x00 })]
    [TestCase((ushort)1000, new byte[] { 0x06, 0x00, 0xA3, 0x00, 0xE8, 0x03 })]
    public void LocoNetDispatchAddress_WritesLittleEndianAddress(ushort locoAddress, byte[] expected)
    {
      LocoNetDispatchAddressCommand command = Factory.Create<LocoNetDispatchAddressCommand>(locoAddress);
      Assert.That(command.Data, Is.EqualTo(expected));
    }

    [Test]
    public void LocoNetDetector_MatchesSpecExample()
    {
      LocoNetDetectorCommand command = Factory.Create<LocoNetDetectorCommand>((byte)0x81, (ushort)1016);
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x07, 0x00, 0xA4, 0x00, 0x81, 0xF8, 0x03 }));
    }
  }
}
