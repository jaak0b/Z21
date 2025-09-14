namespace Z21.Core.Model
{
  public static class Z21BroadcastFlags
  {
    /// <summary>
    /// Automatically generated broadcasts and messages related to driving and switching are delivered to the registered client.
    /// The client needs to be subscribed to a locomotive address to receive messages.
    /// </summary>
    public const uint DriveAndSwitchingMessages = 0x00000001;

    /// <summary>
    /// Changes to feedback sensors on the R-Bus are sent automatically.
    /// </summary>
    public const uint RmBusDataChangedMessages = 0x00000002;

    /// <summary>
    /// Changes in RailCom data of subscribed locomotives are sent automatically.
    /// </summary>
    public const uint RailComDataChangedMessages = 0x00000004;

    /// <summary>
    /// Changes in the Z21 system status are sent automatically.
    /// </summary>
    public const uint SystemStateDataChangedMessages = 0x00000100;

    /// <summary>
    /// The client now receives loco info changed messages without having to subscribe to the corresponding locomotive addresses beforehand. Creates a lot of traffic, should only be used on pcs.
    /// </summary>
    public const uint LocoInfoChangedMessages = 0x00010000;
  }
}