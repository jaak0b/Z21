namespace Z21.Core.Model.EventArgs
{
  public class HardwareInfoEventArgs(int z21HardwareType, int firmwareVersion) : System.EventArgs
  {
    public int Z21HardwareType { get; init; } = z21HardwareType;

    /// <summary>
    /// Raw 32-bit firmware version from the HWINFO reply (BCD; e.g. <c>0x0143</c> means firmware 1.43).
    /// </summary>
    public int FirmwareVersion { get; init; } = firmwareVersion;
  }
}