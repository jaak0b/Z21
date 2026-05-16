using Autofac;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Autofac.DependencyInjection;
using Serilog.Sinks.SystemConsole.Themes;
using Spectre.Console.Cli;
using Z21.Autofac;
using Z21.Console.Command;
using Z21.Core;

namespace Z21.Console
{
  abstract internal class Program
  {
    internal static IZ21Client Z21Client = null!;

    public static void Main(string[] args)
    {
      var log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug, theme: AnsiConsoleTheme.Sixteen);
      
      var builder = new ContainerBuilder();
      builder.AddZ21();
      builder.RegisterSerilog(log);
      var container = builder.Build();     
      
      Z21Client = container.Resolve<IZ21Client>();
      
      Z21Client.ConnectAsync();
      
      CommandApp app = new();
      
      app.Configure(
                    config =>
                    {
                      config.AddCommand<CliSetTrackPowerCommand>("SetTrackPower")
                            .WithDescription("Turn track power on or off.");
      
                      config.AddCommand<CliGetFirmwareVersionCommand>("GetFirmwareVersion")
                            .WithDescription("Retrieve the firmware version.");
                    });
      
      
      app.Run(new List<string>());
      while (true)
      {
        string? value = System.Console.ReadLine();
        if (string.IsNullOrWhiteSpace(value))
          break;
        IEnumerable<string> userArgs = System.CommandLine.Parsing.CommandLineParser.SplitCommandLine(value);
        app.Run(userArgs);
      }
    }
  }
}