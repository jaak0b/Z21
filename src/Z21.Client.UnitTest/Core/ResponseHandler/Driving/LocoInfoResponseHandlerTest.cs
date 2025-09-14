using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler.Driving;

namespace Z21.UnitTest.Core.ResponseHandler.Driving
{
  public class LocoInfoResponseHandlerTest
  {
    private LocoInfoResponseHandler _handler;

    [SetUp]
    public void Setup()
    {
      _handler = new();
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x0F, 0x00, 0x40, 0x00, 0xEF, 0x00, 0x03, 0x02, 0x87, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x0F, 0x00, 0x41, 0x00, 0xEF, 0x00, 0x03, 0x02, 0x87, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69 }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    //TODO add even more tests.
    [Test]
    [TestCase(3,
              DccSpeedMode.Steps28,
              DecoderMode.DCC,
              DrivingDirection.Forward,
              (ushort)11,
              false,
              false,
              false,
              new byte[] { 0x0F, 0x00, 0x40, 0x00, 0xEF, 0x00, 0x03, 0x02, 0x87, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69 })]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs(short locoAddress, DccSpeedMode dccSpeedMode, DecoderMode decoderMode, DrivingDirection drivingDirection, ushort locoSpeed, bool locoIsBusy,
                                                                bool locoContainedInDoubleTraction, bool smartSearch, byte[] response)
    {
      LocoInfoReceivedEventArgs? receivedArgs = null;
      LocoInfoResponseHandler? handler = null;
      _handler.OnLocoInfoReceived += (sender, args) =>
                                     {
                                       receivedArgs = args;
                                       handler = sender as LocoInfoResponseHandler;
                                     };

      _handler.Handle(response);

      Assert.That(handler, Is.EqualTo(_handler));
      Assert.That(receivedArgs, Is.Not.Null);

      Assert.That(receivedArgs.Data.LocoAddress, Is.EqualTo(locoAddress));
      Assert.That(receivedArgs.Data.DccSpeedMode, Is.EqualTo(dccSpeedMode));
      Assert.That(receivedArgs.Data.DecoderMode, Is.EqualTo(decoderMode));
      Assert.That(receivedArgs.Data.DrivingDirection, Is.EqualTo(drivingDirection));
      Assert.That(receivedArgs.Data.LocoSpeed, Is.EqualTo(locoSpeed));
      Assert.That(receivedArgs.Data.LocoIsBusy, Is.EqualTo(locoIsBusy));
      Assert.That(receivedArgs.Data.LocoContainedInDoubleTraction, Is.EqualTo(locoContainedInDoubleTraction));
      Assert.That(receivedArgs.Data.SmartSearch, Is.EqualTo(smartSearch));

      Assert.That(receivedArgs.Data.LocoFunctionsData.All(data => data.FunctionToggleType == FunctionToggleType.Off), Is.True);

      List<short> index = receivedArgs.Data.LocoFunctionsData.Select(data => data.FunctionIndex).Distinct().ToList();
      Assert.That(index, Has.Count.EqualTo(29));
      Assert.That(index.Min(), Is.EqualTo(0));
      Assert.That(index.Max(), Is.EqualTo(28));
    }
  }
}