using System.Collections.Generic;

namespace Z21.Core.Model.EventArgs
{
  public class LocoInfoReceivedEventArgs(LocoInfoData data) : System.EventArgs
  {
   public LocoInfoData Data { get; } = data;
  }
}