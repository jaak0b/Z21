using Z21.Core.Framing;

namespace Z21.Core.Command.Booster
{
  /// <summary>
  /// From booster FW V1.11, disables/re-enables a zLink booster output (<c>LAN_BOOSTER_SET_POWER</c>,
  /// protocol §11.2.5). Port 0x01 = first output, 0x02 = second (dual only), 0x03 = all; state 0x00 = off,
  /// 0x01 = on.
  /// </summary>
  public class SetBoosterPowerCommand : IZ21Command
  {
    public SetBoosterPowerCommand(IZ21FrameBuilder frameBuilder, byte port, byte state)
    {
      Data = frameBuilder.BuildLan(0x00B2, port, state);
    }

    public string Name => "LAN_BOOSTER_SET_POWER";

    public byte[] Data { get; }
  }
}
