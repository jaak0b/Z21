using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Extensions.DependencyInjection;
using Z21.Console.Command;
using Z21.Core;
using Z21.Core.Model;
using Z21.DependencyInjection;

namespace Z21.Console
{
  abstract internal class Program
  {
    internal static IZ21Client Z21Client = null!;

    public static void Main(string[] args)
    {
      Log.Logger = new LoggerConfiguration()
                  .MinimumLevel.Debug()
                  .Enrich.FromLogContext()
                  .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug, theme: AnsiConsoleTheme.Sixteen)
                  .CreateLogger();

      ServiceCollection services = new();
      services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
      services.ConfigureZ21Client(Z21Configuration.Defaults.IpEndPoint);
      services.AddZ21Client();
      services.AddZ21Transport();
      services.AddZ21ResponseParser();
      services.AddZ21ResponseHandler();
      ServiceProvider serviceProvider = services.BuildServiceProvider();

      Z21Client = serviceProvider.GetRequiredService<IZ21Client>();

      Z21Client.ConnectAsync();

      using DependencyInjectionRegistrar registrar = new(services);
      CommandApp app = new(registrar);

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