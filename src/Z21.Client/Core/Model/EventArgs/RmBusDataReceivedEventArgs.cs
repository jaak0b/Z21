namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries an R-BUS feedback status change (<c>LAN_RMBUS_DATACHANGED</c>): the group index and the ten
  /// status bytes (one byte per feedback module, one bit per input).
  /// </summary>
  public class RmBusDataReceivedEventArgs(byte groupIndex, byte[] feedbackStates) : System.EventArgs
  {
    public byte GroupIndex { get; } = groupIndex;

    public byte[] FeedbackStates { get; } = feedbackStates;
  }
}
