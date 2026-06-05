using Z21.Core.Model;

namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries a 10836 switch decoder system state (<c>LAN_DECODER_SYSTEMSTATE_DATACHANGED</c>).
  /// </summary>
  public class SwitchDecoderSystemStateReceivedEventArgs(SwitchDecoderSystemState state) : System.EventArgs
  {
    public SwitchDecoderSystemState State { get; } = state;
  }
}
