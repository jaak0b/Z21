namespace Z21.Core.Model.EventArgs
{
  public class UnknownCommandReceivedEventArgs(byte[] unknownDatagram) : System.EventArgs
  {
    public byte[] UnknownDatagram { get; } = unknownDatagram;
  }
}