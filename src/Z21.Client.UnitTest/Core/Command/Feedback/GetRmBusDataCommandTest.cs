using Z21.Core.Command.Feedback;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Feedback
{
  public class GetRmBusDataCommandTest : CommandTestFixture
  {
    [Test]
    [TestCase((byte)0, new byte[] { 0x05, 0x00, 0x81, 0x00, 0x00 })]
    [TestCase((byte)1, new byte[] { 0x05, 0x00, 0x81, 0x00, 0x01 })]
    public void Ctor_SetsCorrectDataBits(byte groupIndex, byte[] expected)
    {
      GetRmBusDataCommand command = Factory.Create<GetRmBusDataCommand>(groupIndex);
      Assert.That(command.Data, Is.EqualTo(expected));
    }
  }
}
