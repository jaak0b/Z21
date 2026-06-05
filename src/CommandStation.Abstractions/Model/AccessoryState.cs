using System;

namespace CommandStation.Model
{
  [Flags]
  public enum AccessoryState
  {
    Deactivate = 0x0,
    Activate = 0x8
  }
}
