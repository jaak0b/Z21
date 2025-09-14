namespace Z21.Core.Model.EventArgs
{
  public class ExtAccessoryInfoReceivedEventArgs(ushort accessoryAddress, byte encodedState, bool dataValid) : System.EventArgs
  {
    public ushort AccessoryAddress { get; } = accessoryAddress;

    public byte EncodedState { get; } = encodedState;

    public bool DataValid { get; } = dataValid;
  }
}