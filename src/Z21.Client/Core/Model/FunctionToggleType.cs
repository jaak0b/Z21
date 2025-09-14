using System;

namespace Z21.Core.Model
{
  [Flags]
  public enum FunctionToggleType
  {
    Off = 0x0,
    On = 0x40,
    Toggle = 0x80
  }
}