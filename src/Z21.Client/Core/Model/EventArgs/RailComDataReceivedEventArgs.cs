namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries parsed RailCom data (<c>LAN_RAILCOM_DATACHANGED</c>).
  /// </summary>
  public class RailComDataReceivedEventArgs(RailComData data) : System.EventArgs
  {
    public RailComData Data { get; } = data;
  }
}
