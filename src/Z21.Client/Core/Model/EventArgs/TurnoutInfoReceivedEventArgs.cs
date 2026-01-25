namespace Z21.Core.Model.EventArgs
{
  public class TurnoutInfoReceivedEventArgs(ushort accessoryAddress, AccessoryOutput? accessoryOutput) : System.EventArgs
  {

    public ushort AccessoryAddress { get; set; } = accessoryAddress;

    /// <summary>
    /// State of the turnout. Null if not yet switched or switched with an invalid combination.
    /// </summary>
    public AccessoryOutput? AccessoryOutput { get; set; } = accessoryOutput;
  }
}