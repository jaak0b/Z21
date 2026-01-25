namespace Z21.Core.Model.EventArgs
{
  public class ConnectionChangedEventArgs(bool isConnected) : System.EventArgs
  {
    public bool IsConnected { get; } = isConnected;
  }
}