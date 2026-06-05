using System.Collections.Generic;
using Z21.Core.ResponseParser;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.SystemState;
using Z21.Core.ResponseHandler.SystemState.TrackPower;

namespace Z21.UnitTest.Core.ResponseHandler
{
  public class PreExistingHandlerContractTest
  {
    private static IEnumerable<TestCaseData> Handlers()
    {
      TestCaseData Case(string name, IZ21ResponseHandler handler, byte[] valid, int xHeaderIndex) =>
        new TestCaseData(handler, valid, xHeaderIndex).SetName(name);

      yield return Case("TrackShort", new TrackShortResponseHandler(), new byte[] { 0x00, 0x00, 0x40, 0x00, 0x61, 0x08 }, 4);
      yield return Case("TrackPowerOn", new TrackPowerOnResponseHandler(), new byte[] { 0x00, 0x00, 0x40, 0x00, 0x61, 0x01 }, 4);
      yield return Case("TrackPowerOff", new TrackPowerOffResponseHandler(), new byte[] { 0x00, 0x00, 0x40, 0x00, 0x61, 0x00 }, 4);
      yield return Case("ProgrammingMode", new ProgrammingModeResponseHandler(), new byte[] { 0x00, 0x00, 0x40, 0x00, 0x61, 0x02 }, 4);
      yield return Case("Stopped", new StoppedResponseHandler(), new byte[] { 0x00, 0x00, 0x40, 0x00, 0x81, 0x00 }, 4);
      yield return Case("UnknownCommand", new UnknownCommandResponseHandler(), new byte[] { 0x00, 0x00, 0x40, 0x00, 0x61, 0x82 }, 4);
      yield return Case("Version", new VersionResponseHandler(), new byte[] { 0x00, 0x00, 0x40, 0x00, 0x63, 0x21 }, 4);
      yield return Case("Firmware", new FirmwareVersionResponseHandler(), new byte[] { 0x00, 0x00, 0x40, 0x00, 0xF3, 0x0A, 0x00, 0x00, 0xF9 }, 4);
      yield return Case("StatusChanged", new StatusChangedResponseHandler(new CentralStateResponseParser()), new byte[] { 0x00, 0x00, 0x40, 0x00, 0x62, 0x22, 0x00, 0x40 }, 4);
    }

    [TestCaseSource(nameof(Handlers))]
    public void Handler_NameIsNotEmpty(IZ21ResponseHandler handler, byte[] valid, int xHeaderIndex)
    {
      Assert.That(handler.Name, Is.Not.Empty);
    }

    [TestCaseSource(nameof(Handlers))]
    public void CanHandle_ValidFrame_ReturnsTrue(IZ21ResponseHandler handler, byte[] valid, int xHeaderIndex)
    {
      Assert.That(handler.CanHandle(valid), Is.True);
    }

    [TestCaseSource(nameof(Handlers))]
    public void CanHandle_TooShortFrame_ReturnsFalse(IZ21ResponseHandler handler, byte[] valid, int xHeaderIndex)
    {
      Assert.That(handler.CanHandle([0x00, 0x00]), Is.False);
    }

    [TestCaseSource(nameof(Handlers))]
    public void CanHandle_WrongLanHeader_ReturnsFalse(IZ21ResponseHandler handler, byte[] valid, int xHeaderIndex)
    {
      byte[] wrong = (byte[])valid.Clone();
      wrong[2] = 0x41;
      Assert.That(handler.CanHandle(wrong), Is.False);
    }

    [TestCaseSource(nameof(Handlers))]
    public void CanHandle_WrongXHeader_ReturnsFalse(IZ21ResponseHandler handler, byte[] valid, int xHeaderIndex)
    {
      byte[] wrong = (byte[])valid.Clone();
      wrong[xHeaderIndex] ^= 0xFF;
      Assert.That(handler.CanHandle(wrong), Is.False);
    }

    [TestCaseSource(nameof(Handlers))]
    public void CanHandle_WrongDb0_ReturnsFalse(IZ21ResponseHandler handler, byte[] valid, int xHeaderIndex)
    {
      byte[] wrong = (byte[])valid.Clone();
      wrong[xHeaderIndex + 1] ^= 0xFF;
      Assert.That(handler.CanHandle(wrong), Is.False);
    }
  }
}
