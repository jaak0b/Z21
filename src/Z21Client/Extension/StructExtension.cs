using System;

namespace Z21.Extension
{
  public static class StructExtension
  {
    public static bool IsSet(this uint mask, uint flag) => (flag & mask) != 0;
  }
}