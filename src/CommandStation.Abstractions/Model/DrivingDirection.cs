using System;

namespace CommandStation.Model
{
  [Flags]
  public enum DrivingDirection
  {
    Backward = 0x0,
    Forward = 0x80,
  }
}
