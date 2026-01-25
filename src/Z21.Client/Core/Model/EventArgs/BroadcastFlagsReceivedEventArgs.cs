namespace Z21.Core.Model.EventArgs
{
  public class BroadcastFlagsReceivedEventArgs(uint broadCastFlag) : System.EventArgs
  {
    public uint BroadCastFlag { get; } = broadCastFlag;
  }
}