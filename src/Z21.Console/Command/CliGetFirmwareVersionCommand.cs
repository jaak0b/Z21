using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;
using Z21.Core;
using Z21.Core.Command;
using Z21.Core.Command.SystemState;

namespace Z21.Console.Command
{
  public class GetFirmwareVersionSettings : CommandSettings
  {
  }

  public class CliGetFirmwareVersionCommand : Command<GetFirmwareVersionSettings>
  {
    override public int Execute([NotNull] CommandContext context, [NotNull] GetFirmwareVersionSettings settings)
    {
      Program.Z21Client.SendCommandsAsync(new GetFirmwareVersionCommand());
      return 0;
    }
  }
}