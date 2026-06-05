using Z21.Core.Model;

namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries a CAN occupancy detector report (<c>LAN_CAN_DETECTOR</c>).
  /// </summary>
  public class CanDetectorReceivedEventArgs(CanDetectorData data) : System.EventArgs
  {
    public CanDetectorData Data { get; } = data;
  }
}
