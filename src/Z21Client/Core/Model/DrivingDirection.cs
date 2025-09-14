using System;

namespace Z21.Core.Model
{
  [Flags]
  public enum DrivingDirection
  {
    Backward = 0x0,
    Forward = 0x80,
  }
}