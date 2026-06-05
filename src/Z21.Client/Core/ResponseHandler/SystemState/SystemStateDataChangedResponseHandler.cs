using System;
using Z21.Core.Command.SystemState;
using Z21.Core.Model;
using Z21.Core.Model.EventArgs;
using Z21.Core.ResponseParser;

namespace Z21.Core.ResponseHandler.SystemState
{
  public interface ISystemStateDataChangedResponseHandler : IZ21ResponseHandler
  {
    event EventHandler<SystemStatusChangedReceivedEventArgs>? OnSystemStateDataChangedReceived;
  }

  /// <summary>
  /// Reports a change in the system status from the Z21 to the client either automatically when <see cref="Z21BroadcastFlags.SystemStateDataChangedMessages"/> is set via <see cref="SetBroadcastFlagsCommand"/> or when requested via <see cref="GetSystemStateDataCommand"/>.
  /// </summary>
  public class SystemStateDataChangedResponseHandler(ISystemStateResponseParser systemStateResponseParser) : ISystemStateDataChangedResponseHandler
  {
    public string Name => "LAN_SYSTEMSTATE_DATACHANGED";

    public event EventHandler<SystemStatusChangedReceivedEventArgs>? OnSystemStateDataChangedReceived;

    public bool CanHandle(byte[] response) =>
      ((IZ21ResponseHandler)this).MatchesFrame(response, 4, (2, 0x84), (3, 0x00));

    public void Handle(byte[] response)
    {
      byte[] data = new byte[16];
      Buffer.BlockCopy(response, 4, data, 0, 16);
      
      OnSystemStateDataChangedReceived?.Invoke(this, new (systemStateResponseParser.Parse(data)));
    }
  }
}