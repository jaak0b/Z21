using System;

namespace Z21.Core.Model
{

  [Flags]
  public enum AccessoryState
  {
    Deactivate = 0x0,
    Activate = 0x8
  }
}