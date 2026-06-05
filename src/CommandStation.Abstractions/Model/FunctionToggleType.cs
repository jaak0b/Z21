using System;

namespace CommandStation.Model
{
  [Flags]
  public enum FunctionToggleType
  {
    Off = 0x0,
    On = 0x40,
    Toggle = 0x80
  }
}
