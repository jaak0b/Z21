using System;

namespace Z21.Core.Model
{
  /// <summary>
  /// RailCom data option flags (<c>LAN_RAILCOM_DATACHANGED</c> DB Options) indicating which optional
  /// fields the decoder reported.
  /// </summary>
  [Flags]
  public enum RailComOptions : byte
  {
    None = 0x00,

    /// <summary>CH7 subindex 0 speed is present.</summary>
    Speed1 = 0x01,

    /// <summary>CH7 subindex 1 speed is present.</summary>
    Speed2 = 0x02,

    /// <summary>CH7 subindex 7 quality of service is present.</summary>
    QoS = 0x04
  }
}
