namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries the description of a zLink decoder (<c>LAN_DECODER_GET_DESCRIPTION</c> reply).
  /// </summary>
  public class DecoderDescriptionReceivedEventArgs(string name) : System.EventArgs
  {
    public string Name { get; } = name;
  }
}
