using System;
using Z21.Core.Command.Booster;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Booster
{
  public class BoosterCommandTest : CommandTestFixture
  {
    [Test]
    public void GetDescription_BuildsRequest()
    {
      GetBoosterDescriptionCommand command = Factory.Create<GetBoosterDescriptionCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x04, 0x00, 0xB8, 0x00 }));
    }

    [Test]
    public void SetDescription_PadsNameToThirtyTwoBytes()
    {
      SetBoosterDescriptionCommand command = Factory.Create<SetBoosterDescriptionCommand>("AB");
      byte[] expected = new byte[36];
      expected[0] = 0x24;
      expected[2] = 0xB9;
      expected[4] = 0x41;
      expected[5] = 0x42;
      Assert.That(command.Data, Is.EqualTo(expected));
    }

    [Test]
    [TestCase("a\"b")]
    [TestCase("a\\b")]
    public void SetDescription_RejectsForbiddenCharacters(string name)
    {
      ArgumentException exception = Assert.Throws<ArgumentException>(() => Factory.Create<SetBoosterDescriptionCommand>(name))!;
      Assert.That(exception.Message, Does.Contain("not allowed"));
    }

    [Test]
    public void SetDescription_LongName_IsTruncatedToThirtyTwoBytes()
    {
      SetBoosterDescriptionCommand command = Factory.Create<SetBoosterDescriptionCommand>(new string('X', 40));
      Assert.That(command.Data, Has.Length.EqualTo(36), "frame stays 4 header + 32 name bytes even for an over-long name");
    }

    [Test]
    public void SetPower_WritesPortAndState()
    {
      SetBoosterPowerCommand command = Factory.Create<SetBoosterPowerCommand>((byte)0x03, (byte)0x01);
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x06, 0x00, 0xB2, 0x00, 0x03, 0x01 }));
    }

    [Test]
    public void GetSystemState_BuildsRequest()
    {
      GetBoosterSystemStateCommand command = Factory.Create<GetBoosterSystemStateCommand>();
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x04, 0x00, 0xBB, 0x00 }));
    }
  }
}
