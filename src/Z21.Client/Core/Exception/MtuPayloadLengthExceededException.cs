using System;

namespace Z21.Core.Exception
{
  public class MtuPayloadLengthExceededException(string message) : InvalidOperationException(message)
  {
    public static void ThrowIfExceeded(byte[] datagram)
    {
      if (datagram.Length > Z21Client.MaxUdpPayload)
        throw new MtuPayloadLengthExceededException($"Combined UDP payload length '{datagram.Length}' exceeds MTU size '{Z21Client.MaxUdpPayload}'.");
    }
  }
}