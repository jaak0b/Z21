namespace Z21.Core.Model
{
  /// <summary>
  /// Identifies a locomotive function group for <c>LAN_X_SET_LOCO_FUNCTION_GROUP</c>. The enum value is
  /// the wire "Group" byte; each group carries up to 8 functions in a single command (see protocol §4.3.2).
  /// </summary>
  public enum LocoFunctionGroup : byte
  {
    /// <summary>F0, F4, F3, F2, F1.</summary>
    Group1 = 0x20,

    /// <summary>F5–F8.</summary>
    Group2 = 0x21,

    /// <summary>F9–F12.</summary>
    Group3 = 0x22,

    /// <summary>F13–F20.</summary>
    Group4 = 0x23,

    /// <summary>F21–F28.</summary>
    Group5 = 0x28,

    /// <summary>F29–F36.</summary>
    Group6 = 0x29,

    /// <summary>F37–F44.</summary>
    Group7 = 0x2A,

    /// <summary>F45–F52.</summary>
    Group8 = 0x2B,

    /// <summary>F53–F60.</summary>
    Group9 = 0x50,

    /// <summary>F61–F68.</summary>
    Group10 = 0x51
  }
}
