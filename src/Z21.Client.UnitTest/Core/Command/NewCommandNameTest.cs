using CommandStation.Model;
using Z21.Core.Command.Booster;
using Z21.Core.Command.Can;
using Z21.Core.Command.Decoder;
using Z21.Core.Command.Driving;
using Z21.Core.Command.FastClock;
using Z21.Core.Command.Feedback;
using Z21.Core.Command.LocoNet;
using Z21.Core.Command.Programming;
using Z21.Core.Command.RailCom;
using Z21.Core.Command.ZLink;
using Z21.Core.Model;

namespace Z21.UnitTest.Core.Command
{
  public class NewCommandNameTest : CommandTestFixture
  {
    [Test]
    public void Commands_ExposeTheirProtocolName()
    {
      Assert.Multiple(() =>
                      {
                        Assert.That(Factory.Create<SetLocoFunctionGroupCommand>((ushort)3, LocoFunctionGroup.Group1, (byte)0x10).Name, Is.EqualTo("LAN_X_SET_LOCO_FUNCTION_GROUP"));
                        Assert.That(Factory.Create<SetLocoBinaryStateCommand>((ushort)3, (ushort)29, true).Name, Is.EqualTo("LAN_X_SET_LOCO_BINARY_STATE"));
                        Assert.That(Factory.Create<CvReadCommand>((ushort)0).Name, Is.EqualTo("LAN_X_CV_READ"));
                        Assert.That(Factory.Create<CvWriteCommand>((ushort)0, (byte)0).Name, Is.EqualTo("LAN_X_CV_WRITE"));
                        Assert.That(Factory.Create<DccReadRegisterCommand>((byte)1).Name, Is.EqualTo("LAN_X_DCC_READ_REGISTER"));
                        Assert.That(Factory.Create<DccWriteRegisterCommand>((byte)1, (byte)1).Name, Is.EqualTo("LAN_X_DCC_WRITE_REGISTER"));
                        Assert.That(Factory.Create<MmWriteByteCommand>((byte)0, (byte)0).Name, Is.EqualTo("LAN_X_MM_WRITE_BYTE"));
                        Assert.That(Factory.Create<CvPomWriteByteCommand>((ushort)3, (ushort)0, (byte)0).Name, Is.EqualTo("LAN_X_CV_POM_WRITE_BYTE"));
                        Assert.That(Factory.Create<CvPomWriteBitCommand>((ushort)3, (ushort)0, (byte)0, true).Name, Is.EqualTo("LAN_X_CV_POM_WRITE_BIT"));
                        Assert.That(Factory.Create<CvPomReadByteCommand>((ushort)3, (ushort)0).Name, Is.EqualTo("LAN_X_CV_POM_READ_BYTE"));
                        Assert.That(Factory.Create<CvPomAccessoryWriteByteCommand>((ushort)1, true, (byte)0, (ushort)0, (byte)0).Name, Is.EqualTo("LAN_X_CV_POM_ACCESSORY_WRITE_BYTE"));
                        Assert.That(Factory.Create<CvPomAccessoryWriteBitCommand>((ushort)1, true, (byte)0, (ushort)0, (byte)0, true).Name, Is.EqualTo("LAN_X_CV_POM_ACCESSORY_WRITE_BIT"));
                        Assert.That(Factory.Create<CvPomAccessoryReadByteCommand>((ushort)1, true, (byte)0, (ushort)0).Name, Is.EqualTo("LAN_X_CV_POM_ACCESSORY_READ_BYTE"));
                        Assert.That(Factory.Create<GetRmBusDataCommand>((byte)0).Name, Is.EqualTo("LAN_RMBUS_GETDATA"));
                        Assert.That(Factory.Create<ProgramRmBusModuleCommand>((byte)0).Name, Is.EqualTo("LAN_RMBUS_PROGRAMMODULE"));
                        Assert.That(Factory.Create<GetRailComDataCommand>((ushort)3).Name, Is.EqualTo("LAN_RAILCOM_GETDATA"));
                        Assert.That(Factory.Create<LocoNetFromLanCommand>(new byte[] { 0xB0 }).Name, Is.EqualTo("LAN_LOCONET_FROM_LAN"));
                        Assert.That(Factory.Create<LocoNetDispatchAddressCommand>((ushort)3).Name, Is.EqualTo("LAN_LOCONET_DISPATCH_ADDR"));
                        Assert.That(Factory.Create<LocoNetDetectorCommand>((byte)0x81, (ushort)1016).Name, Is.EqualTo("LAN_LOCONET_DETECTOR"));
                        Assert.That(Factory.Create<GetCanDetectorCommand>((ushort)0xD000).Name, Is.EqualTo("LAN_CAN_DETECTOR"));
                        Assert.That(Factory.Create<GetCanDeviceDescriptionCommand>((ushort)0xC101).Name, Is.EqualTo("LAN_CAN_DEVICE_GET_DESCRIPTION"));
                        Assert.That(Factory.Create<SetCanDeviceDescriptionCommand>((ushort)0xC101, "AB").Name, Is.EqualTo("LAN_CAN_DEVICE_SET_DESCRIPTION"));
                        Assert.That(Factory.Create<SetCanBoosterTrackPowerCommand>((ushort)0xC101, (byte)0xFF).Name, Is.EqualTo("LAN_CAN_BOOSTER_SET_TRACKPOWER"));
                        Assert.That(Factory.Create<FastClockControlCommand>(FastClockAction.Read).Name, Is.EqualTo("LAN_FAST_CLOCK_CONTROL"));
                        Assert.That(Factory.Create<GetFastClockSettingsCommand>().Name, Is.EqualTo("LAN_FAST_CLOCK_SETTINGS_GET"));
                        Assert.That(Factory.Create<SetFastClockSettingsCommand>((byte)0x4F).Name, Is.EqualTo("LAN_FAST_CLOCK_SETTINGS_SET"));
                        Assert.That(Factory.Create<SetFastClockSettingsWithRateCommand>((byte)0x4F, (byte)1).Name, Is.EqualTo("LAN_FAST_CLOCK_SETTINGS_SET"));
                        Assert.That(Factory.Create<SetFastClockSettingsWithStartTimeCommand>((byte)0x4F, (byte)1, (byte)0, (byte)0).Name, Is.EqualTo("LAN_FAST_CLOCK_SETTINGS_SET"));
                        Assert.That(Factory.Create<GetBoosterDescriptionCommand>().Name, Is.EqualTo("LAN_BOOSTER_GET_DESCRIPTION"));
                        Assert.That(Factory.Create<SetBoosterDescriptionCommand>("AB").Name, Is.EqualTo("LAN_BOOSTER_SET_DESCRIPTION"));
                        Assert.That(Factory.Create<SetBoosterPowerCommand>((byte)0x03, (byte)0x01).Name, Is.EqualTo("LAN_BOOSTER_SET_POWER"));
                        Assert.That(Factory.Create<GetBoosterSystemStateCommand>().Name, Is.EqualTo("LAN_BOOSTER_SYSTEMSTATE_GETDATA"));
                        Assert.That(Factory.Create<GetDecoderDescriptionCommand>().Name, Is.EqualTo("LAN_DECODER_GET_DESCRIPTION"));
                        Assert.That(Factory.Create<SetDecoderDescriptionCommand>("AB").Name, Is.EqualTo("LAN_DECODER_SET_DESCRIPTION"));
                        Assert.That(Factory.Create<GetDecoderSystemStateCommand>().Name, Is.EqualTo("LAN_DECODER_SYSTEMSTATE_GETDATA"));
                        Assert.That(Factory.Create<GetZLinkHardwareInfoCommand>().Name, Is.EqualTo("LAN_ZLINK_GET_HWINFO"));
                      });
    }
  }
}
