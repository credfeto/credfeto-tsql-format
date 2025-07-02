using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Cocona;
using Credfeto.Tsql.Formatter.Cmd.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace Credfeto.Tsql.Formatter.Cmd;

[SuppressMessage(category: "Microsoft.Performance", checkId: "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated by Cocona")]
[SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Instantiated by Cocona")]
internal sealed class Commands
{
    private readonly ILogger<Commands> _logger;

    [SuppressMessage(category: "FunFair.CodeAnalysis", checkId: "FFS0023: Use ILogger rather than ILogger<T>", Justification = "Needed in this case")]
    public Commands(ILogger<Commands> logger)
    {
        this._logger = logger;
    }

    [Command("format", Description = "Update all packages in all repositories")]
    [SuppressMessage(category: "ReSharper", checkId: "UnusedMember.Global", Justification = "Used by Cocona")]
    public Task FormatFilesAsync([Option(name: "work", ['w'], Description = "folder where to clone repositories")] string workFolder)
    {

        this.Done();

        return Task.CompletedTask;
    }

    private void Done()
    {
        this._logger.LogCompleted();
    }
}