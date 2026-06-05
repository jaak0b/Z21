using System.Collections.Generic;

namespace CommandStation.Model
{
  /// <summary>
  /// A feedback bus status snapshot: the group index and one status byte per feedback module
  /// (one bit per input).
  /// </summary>
  public record FeedbackData(byte GroupIndex, IReadOnlyList<byte> States);
}
