using Z21.Core.Model;

namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries a zLink booster system state (<c>LAN_BOOSTER_SYSTEMSTATE_DATACHANGED</c>).
  /// </summary>
  public class BoosterSystemStateReceivedEventArgs(BoosterSystemState state) : System.EventArgs
  {
    public BoosterSystemState State { get; } = state;
  }
}
