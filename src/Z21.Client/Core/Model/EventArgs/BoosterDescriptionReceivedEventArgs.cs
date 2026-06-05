namespace Z21.Core.Model.EventArgs
{
  /// <summary>
  /// Carries the description of a zLink booster (<c>LAN_BOOSTER_GET_DESCRIPTION</c> reply).
  /// </summary>
  public class BoosterDescriptionReceivedEventArgs(string name) : System.EventArgs
  {
    public string Name { get; } = name;
  }
}
