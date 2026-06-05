using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;
using Z21.Core;
using Z21.Core.Command.SystemState.TrackPower;

namespace Z21.Console.Command
{
  public class SetTrackPowerSettings : CommandSettings
  {
    [CommandOption("-o|--on")]
    [Description("Turn track power on.")]
    public bool On { get; set; }

    [CommandOption("-f|--off")]
    [Description("Turn track power off.")]
    public bool Off { get; set; }

    override public ValidationResult Validate()
    {
      if (On && Off)
        return ValidationResult.Error("Cannot specify both --on and --off.");
      if (!On && !Off)
        return ValidationResult.Error("Must specify either --on or --off.");
      return ValidationResult.Success();
    }
  }

  public class CliSetTrackPowerCommand : Command<SetTrackPowerSettings>
  {
    override public int Execute([NotNull] CommandContext context, [NotNull] SetTrackPowerSettings settings)
    {
      if (settings.On)
      {
        Program.Station.TrackPowerOnAsync();
        return 0;
      }

      if (settings.Off)
      {
        Program.Station.TrackPowerOffAsync();
        return 0;
      }

      return 1;
    }
  }
}