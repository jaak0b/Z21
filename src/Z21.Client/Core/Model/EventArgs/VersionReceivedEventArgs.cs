namespace Z21.Core.Model.EventArgs
{
  public class VersionReceivedEventArgs(FirmwareVersion firmwareVersion, int commandStationId) : System.EventArgs
  {
    public FirmwareVersion FirmwareVersion { get; } = firmwareVersion;

    public int CommandStationId { get; } = commandStationId;
  }
}