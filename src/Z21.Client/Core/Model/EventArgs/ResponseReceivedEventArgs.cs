namespace Z21.Core.Model.EventArgs
{
  public class ResponseReceivedEventArgs(byte[] response) : System.EventArgs
  {
    public byte[] Response { get; } = response;
  }
}