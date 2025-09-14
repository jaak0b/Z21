namespace Z21.Core.Model.EventArgs
{
  public class StatusChangedReceivedEventArgs(CentralState centralState)
  {
    public CentralState CentralState { get; } = centralState;
  }
}