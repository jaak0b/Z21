using System;
using Z21.Core.Command.Decoder;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Decoder
{
  public class DecoderCommandTest : CommandTestFixture
  {
    [Test]
    public void GetDescription_BuildsRequest()
    {
      GetDecoderDescriptionCommand command = Factory.Create<GetDecoderDescriptionCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x04, 0x00, 0xD8, 0x00 }));
    }

    [Test]
    public void SetDescription_PadsNameToThirtyTwoBytes()
    {
      SetDecoderDescriptionCommand command = Factory.Create<SetDecoderDescriptionCommand>("AB");
      byte[] expected = new byte[36];
      expected[0] = 0x24;
      expected[2] = 0xD9;
      expected[4] = 0x41;
      expected[5] = 0x42;
      Assert.That(command.Data, Is.EqualTo(expected));
    }

    [Test]
    [TestCase("a\"b")]
    [TestCase("a\\b")]
    public void SetDescription_RejectsForbiddenCharacters(string name)
    {
      Assert.Throws<ArgumentException>(() => Factory.Create<SetDecoderDescriptionCommand>(name));
    }

    [Test]
    public void GetSystemState_BuildsRequest()
    {
      GetDecoderSystemStateCommand command = Factory.Create<GetDecoderSystemStateCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x04, 0x00, 0xDB, 0x00 }));
    }
  }
}
