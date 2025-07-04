using System;
using System.Threading;
using System.Threading.Tasks;
using Cocona;
using Cocona.Builder;
using Credfeto.Tsql.Formatter.Cmd.Constants;
using Credfeto.Tsql.Formatter.Cmd.Setup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Credfeto.Tsql.Formatter.Cmd;

internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        Console.WriteLine($"{VersionInformation.Product} {VersionInformation.Version}");
        Console.WriteLine();

        try
        {
            using (CoconaApp host = CreateApp(args))
            {
                ILoggerFactory loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
                Logging.InitializeLogging(loggerFactory: loggerFactory);

                host.AddCommands<Commands>();

                await host.RunAsync(CancellationToken.None);

                return ExitCodes.Success;
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine($"ERROR: {exception.Message}");

            return ExitCodes.Error;
        }
    }

    private static CoconaApp CreateApp(string[] args)
    {
        CoconaAppBuilder builder = CoconaApp.CreateBuilder(args);
        builder.Services.AddServices();
        builder
            .Logging.AddFilter(category: "Microsoft", level: LogLevel.Warning)
            .AddFilter(category: "System.Net.Http.HttpClient", level: LogLevel.Warning)
            .ClearProviders();

        return builder.Build();
    }
}
