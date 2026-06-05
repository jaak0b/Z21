using System;
using Z21.Core.Command.SystemState;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.SystemState
{
  public interface ISoftwareLockResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<SoftwareLockReceivedEventArgs>? OnSoftwareLockReceived;
  }

  /// <summary>
  /// Reads the software feature scope of the Z21 when request via <see cref="GetSoftwareLockCommand"/>.
  /// </summary>
  public class SoftwareLockResponseHandler : ISoftwareLockResponseHandler
  {
    public string Name => "LAN_GET_CODE";

    public event EventHandler<SoftwareLockReceivedEventArgs>? OnSoftwareLockReceived;

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 4, (2, 0x18), (3, 0x00));

    public void Handle(byte[] response)
    {
      OnSoftwareLockReceived?.Invoke(this, new(response[4]));
    }
  }
}