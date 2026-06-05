namespace Z21.Core.Model
{
  /// <summary>
  /// A parameterless fast-clock control action for <c>LAN_FAST_CLOCK_CONTROL</c> (protocol §12.1).
  /// </summary>
  public enum FastClockAction
  {
    /// <summary>Read the current model time.</summary>
    Read,

    /// <summary>Start (resume) the model clock.</summary>
    Start,

    /// <summary>Stop (pause) the model clock.</summary>
    Stop
  }
}
