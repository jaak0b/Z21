using System.Collections.Generic;

namespace CommandStation.Model
{
  public class LocoInfoData
  {
    public required ushort LocoAddress { get; init; }

    public required IReadOnlyCollection<LocoFunctionData> LocoFunctionsData { get; init; }

    public required DccSpeedMode DccSpeedMode { get; init; }

    public required DecoderMode DecoderMode { get; init; }

    public required DrivingDirection DrivingDirection { get; init; }

    public required ushort LocoSpeed { get; init; }

    public required bool LocoIsBusy { get; init; }

    public required bool LocoContainedInDoubleTraction { get; init; }

    public required bool SmartSearch { get; init; }
  }
}
