using Moq;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseHandler;
using Z21.Core.ResponseHandler.SystemState;
using Z21.Core.ResponseParser;

namespace Z21.UnitTest.Core.ResponseHandler.SystemState
{
  public class SystemStateDataChangedResponseHandlerTest
  {
    private SystemStateDataChangedResponseHandler _handler;
    private Mock<ISystemStateResponseParser> _systemStateResponseParserMock;

    [SetUp]
    public void Setup()
    {
      _systemStateResponseParserMock = new(MockBehavior.Strict);
      _handler = new(_systemStateResponseParserMock.Object);
    }

    [Test]
    public void CanHandle_ValidResponse_ReturnsTrue()
    {
      byte[] validResponse = [0x14, 0x00, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x83, 0x45, 0x83, 0x45, 0x00, 0x00, 0x00, 0x7B];

      bool result = _handler.CanHandle(validResponse);

      Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(new byte[] { 0x14, 0x00, 0x84, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x83, 0x45, 0x83, 0x45, 0x00, 0x00, 0x00, 0x7B }, TestName = "Not Responsible")]
    [TestCase(new byte[] { 0x00 }, TestName = "Response to small")]
    public void CanHandle_InvalidResponse_ReturnsFalse(byte[] response)
    {
      bool result = _handler.CanHandle(response);

      Assert.That(result, Is.False);
    }

    [Test]
    public void Handle_ValidResponse_RaisesEventWithCorrectArgs()
    {
      byte[] response = [0x14, 0x00, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x83, 0x45, 0x83, 0x45, 0x00, 0x00, 0x00, 0x7B];

      SystemStatusChangedReceivedEventArgs? receivedArgs = null;
      SystemStateDataChangedResponseHandler? handler = null;
      _handler.OnSystemStateDataChangedReceived += (sender, args) =>
                                                   {
                                                     receivedArgs = args;
                                                     handler = sender as SystemStateDataChangedResponseHandler;
                                                   };

      Z21.Core.Model.SystemState systemState = new() { CentralState = null!, CentralStateEx = null! };

      _systemStateResponseParserMock.Setup(parser => parser.Parse(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x83, 0x45, 0x83, 0x45, 0x00, 0x00, 0x00, 0x7B }))
                                    .Returns(systemState)
                                    .Verifiable();

      _handler.Handle(response);

      Assert.That(receivedArgs, Is.Not.Null);
      Assert.That(receivedArgs.SystemState, Is.EqualTo(systemState));
      Assert.That(handler, Is.EqualTo(_handler));
      _systemStateResponseParserMock.Verify();
    }
  }
}