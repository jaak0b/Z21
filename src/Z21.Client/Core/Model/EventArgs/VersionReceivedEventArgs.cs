namespace Z21.Core.Model.EventArgs
{
  public class VersionReceivedEventArgs(int xbusVer, int cmdstId) : System.EventArgs
  {
    public int XbusVer { get; } = xbusVer;

    public int CmdstId { get; } = cmdstId;
  }
}