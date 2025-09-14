namespace Z21.Core.Model.EventArgs
{
  public class HardwareInfoEventArgs(int z21HardwareType) : System.EventArgs
  {
    public int Z21HardwareType { get; init; } = z21HardwareType;
  }
}