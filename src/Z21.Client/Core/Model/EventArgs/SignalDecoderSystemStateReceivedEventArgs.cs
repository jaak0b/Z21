using Z21.Core.Model;

namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries a 10837 signal decoder system state (<c>LAN_DECODER_SYSTEMSTATE_DATACHANGED</c>).
  /// </summary>
  public class SignalDecoderSystemStateReceivedEventArgs(SignalDecoderSystemState state) : System.EventArgs
  {
    public SignalDecoderSystemState State { get; } = state;
  }
}
