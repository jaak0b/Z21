using System;

namespace Z21.Core.Model
{
  /// <summary>
  /// CAN booster state bit mask (<c>LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD</c> Booster_State, protocol §10.2.3).
  /// </summary>
  [Flags]
  public enum CanBoosterState : ushort
  {
    None = 0x0000,

    /// <summary>Brake generator active (ZCAN SSP).</summary>
    BrakeGeneratorActive = 0x0001,

    /// <summary>Short circuit at the output stage (ZCAN UES).</summary>
    ShortCircuit = 0x0020,

    /// <summary>Track voltage is switched off.</summary>
    TrackVoltageOff = 0x0080,

    /// <summary>Booster output disabled by the user (from booster FW V1.11).</summary>
    OutputDisabled = 0x0100,

    /// <summary>RailCom cutout active.</summary>
    RailComActive = 0x0800
  }
}
