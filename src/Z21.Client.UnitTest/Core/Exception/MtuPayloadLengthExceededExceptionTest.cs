using Z21.Core;
using Z21.Core.Exception;

namespace Z21.UnitTest.Core.Exception
{
  public class MtuPayloadLengthExceededExceptionTest
  {
    [Test]
    public void ThrowIfExceeded_DatagramSmallerThenMaxUdpPayload_DoNothing()
    {
      byte[] datagram = new byte [Z21CommandStation.MaxUdpPayload - 1];
      Assert.DoesNotThrow(() => MtuPayloadLengthExceededException.ThrowIfExceeded(datagram));
    }

    [Test]
    public void ThrowIfExceeded_DatagramEqualMaxUdpPayload_DoNothing()
    {
      byte[] datagram = new byte [Z21CommandStation.MaxUdpPayload];
      Assert.DoesNotThrow(() => MtuPayloadLengthExceededException.ThrowIfExceeded(datagram));
    }

    [Test]
    public void ThrowIfExceeded_DatagramBiggerThenMaxUdpPayload_ThrowMtuPayloadLengthExceededException()
    {
      byte[] datagram = new byte [Z21CommandStation.MaxUdpPayload + 1];
      MtuPayloadLengthExceededException exception = Assert.Throws<MtuPayloadLengthExceededException>(() => MtuPayloadLengthExceededException.ThrowIfExceeded(datagram));
      Assert.That(exception.Message, Is.EqualTo($"Combined UDP payload length '{datagram.Length}' exceeds MTU size '{Z21CommandStation.MaxUdpPayload}'."));
    }
  }
}