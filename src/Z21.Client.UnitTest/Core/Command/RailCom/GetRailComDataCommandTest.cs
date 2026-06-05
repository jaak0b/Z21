using Z21.Core.Command.RailCom;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.RailCom
{
  public class GetRailComDataCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((ushort)3, new byte[] { 0x07, 0x00, 0x89, 0x00, 0x01, 0x03, 0x00 })]
    [TestCase((ushort)1000, new byte[] { 0x07, 0x00, 0x89, 0x00, 0x01, 0xE8, 0x03 })]
    [TestCase((ushort)0, new byte[] { 0x07, 0x00, 0x89, 0x00, 0x01, 0x00, 0x00 })]
    public void Ctor_SetsCorrectDataBits(ushort locoAddress, byte[] expected)
    {
      GetRailComDataCommand command = Factory.Create<GetRailComDataCommand>(locoAddress);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
