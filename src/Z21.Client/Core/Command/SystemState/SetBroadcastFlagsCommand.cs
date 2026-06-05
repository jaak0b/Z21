using System;
using System.Linq;
using Z21.Core.Framing;
using Z21.Core.Model;

namespace Z21.Core.Command.SystemState
{
  /// <summary>
  /// Implements the LAN_SET_BROADCASTFLAGS command. See <see cref="Z21BroadcastFlags"/> for some flags. See <c>LAN_SET_BROADCASTFLAGS</c> section of the Z21 lan protocol document for all possible flags.
  /// </summary>
  public class SetBroadcastFlagsCommand : IZ21Command
  {
    public SetBroadcastFlagsCommand(IZ21FrameBuilder frameBuilder, params uint[] flags)
    {
      uint flag = flags.Length > 0 ? flags.Aggregate((u, u1) => u | u1) : 0;
      byte[] broadcastFlags = BitConverter.GetBytes(flag);
      Data = frameBuilder.BuildLan(0x0050, broadcastFlags[0], broadcastFlags[1], broadcastFlags[2], broadcastFlags[3]);
    }

    public string Name => "LAN_SET_BROADCASTFLAGS";

    public byte[] Data { get; }
  }
}
