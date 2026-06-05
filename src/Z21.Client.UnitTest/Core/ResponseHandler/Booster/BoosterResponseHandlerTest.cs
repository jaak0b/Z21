using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.Booster;

namespace Z21.UnitTest.Core.ResponseHandler.Booster
{
  public class BoosterResponseHandlerTest
  {
    [Test]
    public void Description_DecodesName()
    {
      BoosterDescriptionResponseHandler handler = new();
      byte[] response = new byte[36];
      response[0] = 0x24;
      response[2] = 0xB8;
      response[4] = 0x41;
      response[5] = 0x42;

      Assert.That(handler.CanHandle(response), Is.True);

      BoosterDescriptionReceivedEventArgs? received = null;
      handler.OnBoosterDescriptionReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.That(received!.Name, Is.EqualTo("AB"));
    }

    [Test]
    public void Description_NeverSet_IsEmptyString()
    {
      BoosterDescriptionResponseHandler handler = new();
      byte[] response = new byte[36];
      response[0] = 0x24;
      response[2] = 0xB8;
      response[4] = 0xFF;

      BoosterDescriptionReceivedEventArgs? received = null;
      handler.OnBoosterDescriptionReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.That(received!.Name, Is.EqualTo(string.Empty));
    }

    [Test]
    public void SystemState_DecodesAllFields()
    {
      BoosterSystemStateResponseHandler handler = new();
      byte[] response =
      [
        0x1C, 0x00, 0xBA, 0x00,
        0x64, 0x00, 0xFF, 0xFF, 0xC8, 0x00, 0x00, 0x00, 0x19, 0x00, 0x00, 0x00,
        0x98, 0x3A, 0x88, 0x13, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00
      ];

      Assert.That(handler.CanHandle(response), Is.True);

      BoosterSystemStateReceivedEventArgs? received = null;
      handler.OnBoosterSystemStateReceived += (_, args) => received = args;
      handler.Handle(response);

      Assert.Multiple(() =>
                      {
                        Assert.That(received!.State.Booster1MainCurrent, Is.EqualTo(100));
                        Assert.That(received.State.Booster2MainCurrent, Is.EqualTo(-1));
                        Assert.That(received.State.Booster1FilteredMainCurrent, Is.EqualTo(200));
                        Assert.That(received.State.Booster1Temperature, Is.EqualTo(25));
                        Assert.That(received.State.SupplyVoltage, Is.EqualTo(15000));
                        Assert.That(received.State.Booster1VccVoltage, Is.EqualTo(5000));
                        Assert.That(received.State.CentralState, Is.EqualTo(0x02));
                      });
    }

    [Test]
    public void SystemState_RejectsOtherHeaders()
    {
      BoosterSystemStateResponseHandler handler = new();
      Assert.That(handler.CanHandle([0x00]), Is.False);
    }
  }
}
