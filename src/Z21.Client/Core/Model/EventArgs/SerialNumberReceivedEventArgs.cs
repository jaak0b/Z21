namespace Z21.Core.Model.EventArgs
{
  public class SerialNumberReceivedEventArgs (uint serialNumber) : System.EventArgs
  {
    public uint SerialNumber { get; } = serialNumber;
  }
}