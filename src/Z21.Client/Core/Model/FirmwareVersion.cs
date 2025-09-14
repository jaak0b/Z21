namespace Z21.Core.Model
{
  public class FirmwareVersion(int major, int minor)
  {
    public int Major { get; } = major;

    public int Minor { get; } = minor;

    public string Firmware { get; } = major + "." + minor;

    override public string ToString() => Firmware;
  }
}