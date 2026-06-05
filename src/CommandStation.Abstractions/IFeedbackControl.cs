using System;
using System.Threading.Tasks;
using CommandStation.Model;

namespace CommandStation
{
  /// <summary>
  /// Reading occupancy/feedback modules. Group index 0 covers module addresses 1–10, group index 1
  /// covers 11–20.
  /// </summary>
  public interface IFeedbackControl
  {
    Task RequestFeedbackAsync(byte groupIndex);

    event EventHandler<FeedbackData>? FeedbackChanged;
  }
}
