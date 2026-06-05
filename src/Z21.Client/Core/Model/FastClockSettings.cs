using System;

namespace Z21.Core.Model
{
  /// <summary>
  /// Persistent fast-clock setting flags (<c>FcSettings</c>, protocol §12.3).
  /// </summary>
  [Flags]
  public enum FastClockSettings : byte
  {
    None = 0x00,

    /// <summary>Enable polled output on the LocoNet.</summary>
    LocoNetEnabled = 0x01,

    /// <summary>Enable the broadcast on the X-BUS.</summary>
    XBusEnabled = 0x02,

    /// <summary>Enable the DCC broadcast on the track.</summary>
    DccEnabled = 0x08,

    /// <summary>Enable the multicast to MRclock clients.</summary>
    MRclockEnabled = 0x10,

    /// <summary>Automatically halt the model time on emergency stop.</summary>
    EmergencyHaltEnabled = 0x40,

    /// <summary>The fast clock is enabled.</summary>
    Enabled = 0x80
  }
}
