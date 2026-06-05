using Z21.Core.Model;

namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries a CAN booster system state (<c>LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD</c>).
  /// </summary>
  public class CanBoosterSystemStateReceivedEventArgs(CanBoosterSystemState state) : System.EventArgs
  {
    public CanBoosterSystemState State { get; } = state;
  }
}
