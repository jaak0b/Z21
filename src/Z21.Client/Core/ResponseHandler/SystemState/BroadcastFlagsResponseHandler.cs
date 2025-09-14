using System;
using Z21.Core.Model.EventArgs;

namespace Z21.Core.ResponseHandler.SystemState
{
  public interface IBroadcastFlagsResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<BroadcastFlagsReceivedEventArgs>? OnBroadcastFlagsReceived;
  }

  /// <summary>
  /// Reading the broadcast flags in the Z21.
  /// </summary>
  public class BroadcastFlagsResponseHandler : IBroadcastFlagsResponseHandler
  {
    public event EventHandler<BroadcastFlagsReceivedEventArgs>? OnBroadcastFlagsReceived;

    public string Name => "LAN_GET_BROADCASTFLAGS";

    public bool CanHandle(byte[] response)
    {
      try
      {
        return response[2] == 0x51 
               && response[3] == 0x00;
      } catch (IndexOutOfRangeException)
      {
        return false;
      }
    }

    public void Handle(byte[] response)
    {
      uint flag = BitConverter.ToUInt32(response, 4);

      OnBroadcastFlagsReceived?.Invoke(this, new(flag));
    }
  }
}