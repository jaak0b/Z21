namespace Z21.Core.Model.EventArgs
{
  public class SoftwareLockReceivedEventArgs(int code) : System.EventArgs
  {
    public int Code { get; } = code;
  }
}