using System;
using Z21.Core.Codecs;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.Programming
{
  public interface ICvResultResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<CvResultReceivedEventArgs>? OnCvResultReceived;
  }

  /// <summary>
  /// Positive acknowledgement of a CV read/write (<c>LAN_X_CV_RESULT</c>, protocol §6.5), sent to the
  /// triggering client.
  /// </summary>
  public class CvResultResponseHandler(IAddressCodec addressCodec) : ICvResultResponseHandler
  {
    public event EventHandler<CvResultReceivedEventArgs>? OnCvResultReceived;

    public string Name => "LAN_X_CV_RESULT";

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 6, (2, 0x40), (3, 0x00), (4, 0x64), (5, 0x14));

    public void Handle(byte[] response)
    {
      ushort cvAddress = addressCodec.CombineCvAddress(response[6], response[7]);
      byte value = response[8];
      OnCvResultReceived?.Invoke(this, new(cvAddress, value));
    }
  }
}
