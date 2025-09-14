using Z21.Core.ResponseHandler;

namespace Z21.Core.Model.EventArgs
{
  public class FirmwareVersionReceivedEventArgs(FirmwareVersion firmwareVersion)
  {
    public FirmwareVersion FirmwareVersion { get; } = firmwareVersion;
  }
}