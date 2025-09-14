namespace Z21.Core.Model.EventArgs
{
  public class DecoderModeReceivedEventArgs(short locoAddress, DecoderMode mode) : System.EventArgs
  {
    public short LocoAddress { get; } = locoAddress;

    public DecoderMode Mode { get; } = mode;
  }
}