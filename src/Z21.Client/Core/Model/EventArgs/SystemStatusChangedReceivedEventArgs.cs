namespace Z21.Core.Model.EventArgs
{
  public class SystemStatusChangedReceivedEventArgs (SystemState systemState)
  {
    public SystemState SystemState { get; } = systemState;
  }
}