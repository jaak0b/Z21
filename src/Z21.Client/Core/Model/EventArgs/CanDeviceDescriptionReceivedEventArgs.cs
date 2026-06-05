namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries the description of a CAN booster (<c>LAN_CAN_DEVICE_GET_DESCRIPTION</c> reply).
  /// </summary>
  public class CanDeviceDescriptionReceivedEventArgs(ushort networkId, string name) : System.EventArgs
  {
    public ushort NetworkId { get; } = networkId;

    public string Name { get; } = name;
  }
}
