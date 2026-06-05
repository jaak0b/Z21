using System;
using System.Collections.Generic;
using Z21.Core.Codecs;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.Booster;
using Z21.Core.ResponseHandler.Can;
using Z21.Core.ResponseHandler.Decoder;
using Z21.Core.ResponseHandler.FastClock;
using Z21.Core.ResponseHandler.Feedback;
using Z21.Core.ResponseHandler.LocoNet;
using Z21.Core.ResponseHandler.Programming;
using Z21.Core.ResponseHandler.RailCom;
using Z21.Core.ResponseHandler.ZLink;
using Z21.Core.ResponseParser;

namespace Z21.UnitTest.Core.ResponseHandler
{
  public class NewHandlerContractTest
  {
    private static byte[] Frame(int length, byte header)
    {
      byte[] frame = new byte[length];
      frame[2] = header;
      frame[3] = 0x00;
      return frame;
    }

    private static IEnumerable<TestCaseData> Handlers()
    {
      TestCaseData Case(string name, IZ21ResponseHandler handler, byte[] valid) => new TestCaseData(handler, valid).SetName(name);

      yield return Case("CvResult", new CvResultResponseHandler(new AddressCodec()), Set(Frame(9, 0x40), (4, 0x64), (5, 0x14)));
      yield return Case("CvNack", new CvNackResponseHandler(), Set(Frame(6, 0x40), (4, 0x61), (5, 0x13)));
      yield return Case("CvNackSc", new CvNackShortCircuitResponseHandler(), Set(Frame(6, 0x40), (4, 0x61), (5, 0x12)));
      yield return Case("RmBus", new RmBusDataChangedResponseHandler(), Frame(15, 0x80));
      yield return Case("RailCom", new RailComDataChangedResponseHandler(new RailComDataParser()), Frame(17, 0x88));
      yield return Case("LocoNetRx", new LocoNetReceiveResponseHandler(), Frame(4, 0xA0));
      yield return Case("LocoNetTx", new LocoNetTransmitResponseHandler(), Frame(4, 0xA1));
      yield return Case("LocoNetFromLan", new LocoNetFromLanResponseHandler(), Frame(4, 0xA2));
      yield return Case("LocoNetDispatch", new LocoNetDispatchAddressResponseHandler(), Frame(7, 0xA3));
      yield return Case("LocoNetDetector", new LocoNetDetectorResponseHandler(), Frame(7, 0xA4));
      yield return Case("CanDetector", new CanDetectorResponseHandler(), Frame(14, 0xC4));
      yield return Case("CanDeviceDescription", new CanDeviceDescriptionResponseHandler(), Frame(22, 0xC8));
      yield return Case("CanBoosterState", new CanBoosterSystemStateResponseHandler(), Frame(14, 0xCA));
      yield return Case("FastClockData", new FastClockDataResponseHandler(), Frame(12, 0xCD));
      yield return Case("FastClockSettings", new FastClockSettingsResponseHandler(), Frame(8, 0xCE));
      yield return Case("BoosterDescription", new BoosterDescriptionResponseHandler(), Frame(36, 0xB8));
      yield return Case("BoosterState", new BoosterSystemStateResponseHandler(), Frame(28, 0xBA));
      yield return Case("DecoderDescription", new DecoderDescriptionResponseHandler(), Frame(36, 0xD8));
      yield return Case("DecoderState", new DecoderSystemStateResponseHandler(new SwitchDecoderSystemStateParser(), new SignalDecoderSystemStateParser()), Frame(48, 0xDA));
      yield return Case("ZLinkHwInfo", new ZLinkHardwareInfoResponseHandler(new ZLinkHardwareInfoParser()), Set(Frame(63, 0xE8), (4, 0x06)));
    }

    private static byte[] Set(byte[] frame, params (int index, byte value)[] overrides)
    {
      foreach ((int index, byte value) in overrides)
        frame[index] = value;
      return frame;
    }

    [TestCaseSource(nameof(Handlers))]
    public void Handler_NameIsNotEmpty(IZ21ResponseHandler handler, byte[] valid)
    {
      Assert.That(handler.Name, Is.Not.Empty);
    }

    [TestCaseSource(nameof(Handlers))]
    public void CanHandle_ValidMinLengthFrame_ReturnsTrue(IZ21ResponseHandler handler, byte[] valid)
    {
      Assert.That(handler.CanHandle(valid), Is.True);
    }

    [TestCaseSource(nameof(Handlers))]
    public void CanHandle_TooShortFrame_ReturnsFalse(IZ21ResponseHandler handler, byte[] valid)
    {
      Assert.That(handler.CanHandle([0x00, 0x00]), Is.False);
    }

    [TestCaseSource(nameof(Handlers))]
    public void CanHandle_WrongHeaderByte_ReturnsFalse(IZ21ResponseHandler handler, byte[] valid)
    {
      byte[] wrong = (byte[])valid.Clone();
      wrong[2] ^= 0xFF;
      Assert.That(handler.CanHandle(wrong), Is.False);
    }

    [TestCaseSource(nameof(Handlers))]
    public void CanHandle_WrongHeaderHighByte_ReturnsFalse(IZ21ResponseHandler handler, byte[] valid)
    {
      byte[] wrong = (byte[])valid.Clone();
      wrong[3] = 0x01;
      Assert.That(handler.CanHandle(wrong), Is.False);
    }
  }
}
