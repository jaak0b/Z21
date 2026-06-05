using Z21.Core.Codecs;
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
      _handler = new(new LocoSpeedCodec());
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
    [TestCase(3,
              DccSpeedMode.Steps14,
              DecoderMode.DCC,
              DrivingDirection.Forward,
              (ushort)6,
              false,
              false,
              false,
              new byte[] { 0x0F, 0x00, 0x40, 0x00, 0xEF, 0x00, 0x03, 0x00, 0x87, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69 },
              TestName = "14 speed steps (KKK=000)")]
    [TestCase(3, DccSpeedMode.Steps14, DecoderMode.MM, DrivingDirection.Forward, (ushort)0, false, false, false,
              new byte[] { 0x0F, 0x00, 0x40, 0x00, 0xEF, 0x00, 0x03, 0x10, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69 },
              TestName = "MM decoder (M bit set)")]
    [TestCase(3, DccSpeedMode.Steps14, DecoderMode.DCC, DrivingDirection.Forward, (ushort)0, true, false, false,
              new byte[] { 0x0F, 0x00, 0x40, 0x00, 0xEF, 0x00, 0x03, 0x08, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69 },
              TestName = "Busy (B bit set)")]
    [TestCase(3, DccSpeedMode.Steps128, DecoderMode.DCC, DrivingDirection.Forward, (ushort)4, false, false, false,
              new byte[] { 0x0F, 0x00, 0x40, 0x00, 0xEF, 0x00, 0x03, 0x04, 0x85, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69 },
              TestName = "128 speed steps (KKK=100)")]
    [TestCase(3, DccSpeedMode.Steps28, DecoderMode.DCC, DrivingDirection.Backward, (ushort)0, false, false, false,
              new byte[] { 0x0F, 0x00, 0x40, 0x00, 0xEF, 0x00, 0x03, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69 },
              TestName = "Backward (R bit clear)")]
    [TestCase(3, DccSpeedMode.Steps28, DecoderMode.DCC, DrivingDirection.Forward, (ushort)11, false, true, true,
              new byte[] { 0x0F, 0x00, 0x40, 0x00, 0xEF, 0x00, 0x03, 0x02, 0x87, 0x60, 0x00, 0x00, 0x00, 0x00, 0x69 },
              TestName = "Double traction + smart search")]
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
      Assert.That(index, Has.Count.EqualTo(37));
      Assert.That(index.Min(), Is.EqualTo(0));
      Assert.That(index.Max(), Is.EqualTo(36));
    }

    [Test]
    public void Handle_Db8FunctionBitsSet_ReportsF29ToF31On()
    {
      // 15-byte frame (DataLen 0x0F): DB8 (the byte immediately before the XOR) = 0x07 => F29, F30, F31 on.
      byte[] response = [0x0F, 0x00, 0x40, 0x00, 0xEF, 0x00, 0x03, 0x02, 0x87, 0x00, 0x00, 0x00, 0x00, 0x07, 0x69];

      LocoInfoReceivedEventArgs? receivedArgs = null;
      _handler.OnLocoInfoReceived += (_, args) => receivedArgs = args;

      _handler.Handle(response);

      Assert.That(receivedArgs, Is.Not.Null);
      var on = receivedArgs!.Data.LocoFunctionsData.Where(d => d.FunctionToggleType == FunctionToggleType.On).Select(d => (int)d.FunctionIndex).ToList();
      Assert.That(on, Is.EquivalentTo(new[] { 29, 30, 31 }));
    }

    [Test]
    public void Handle_FunctionBitsSet_ReportsThoseFunctionsOn()
    {
      // db4 = 0x1F => F0(L), F4, F3, F2, F1 all on; db5 = 0x01 => F5 on.
      byte[] response = [0x0F, 0x00, 0x40, 0x00, 0xEF, 0x00, 0x03, 0x02, 0x87, 0x1F, 0x01, 0x00, 0x00, 0x00, 0x69];

      LocoInfoReceivedEventArgs? receivedArgs = null;
      _handler.OnLocoInfoReceived += (_, args) => receivedArgs = args;

      _handler.Handle(response);

      Assert.That(receivedArgs, Is.Not.Null);
      var on = receivedArgs!.Data.LocoFunctionsData.Where(d => d.FunctionToggleType == FunctionToggleType.On).Select(d => (int)d.FunctionIndex).ToList();
      Assert.That(on, Is.EquivalentTo(new[] { 0, 1, 2, 3, 4, 5 }));
    }
  }
}