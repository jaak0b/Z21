using System;
using Z21.Core.Command.Can;
using Z21.UnitTest.Core.Command;

namespace Z21.UnitTest.Core.Command.Can
{
  public class CanCommandTest : CommandTestFixture
  {
    [Test]
    public void GetCanDetector_MatchesSpecExample()
    {
      GetCanDetectorCommand command = Factory.Create<GetCanDetectorCommand>((ushort)0xD000);
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x07, 0x00, 0xC4, 0x00, 0x00, 0x00, 0xD0 }));
    }

    [Test]
    public void GetCanDeviceDescription_WritesLittleEndianNetworkId()
    {
      GetCanDeviceDescriptionCommand command = Factory.Create<GetCanDeviceDescriptionCommand>((ushort)0xC101);
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x06, 0x00, 0xC8, 0x00, 0x01, 0xC1 }));
    }

    [Test]
    public void SetCanDeviceDescription_PadsNameToSixteenBytes()
    {
      SetCanDeviceDescriptionCommand command = Factory.Create<SetCanDeviceDescriptionCommand>((ushort)0xC101, "AB");
      Assert.That(command.Data, Is.EqualTo(new byte[]
                                           {
                                             0x16, 0x00, 0xC9, 0x00, 0x01, 0xC1,
                                             0x41, 0x42, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                                           }));
    }

    [Test]
    [TestCase("a\"b")]
    [TestCase("a\\b")]
    public void SetCanDeviceDescription_RejectsForbiddenCharacters(string name)
    {
      Assert.Throws<ArgumentException>(() => Factory.Create<SetCanDeviceDescriptionCommand>((ushort)0xC101, name));
    }

    [Test]
    public void SetCanBoosterTrackPower_WritesNetworkIdAndPower()
    {
      SetCanBoosterTrackPowerCommand command = Factory.Create<SetCanBoosterTrackPowerCommand>((ushort)0xC101, (byte)0xFF);
      Assert.That(command.Data, Is.EqualTo(new byte[] { 0x07, 0x00, 0xCB, 0x00, 0x01, 0xC1, 0xFF }));
    }
  }
}
