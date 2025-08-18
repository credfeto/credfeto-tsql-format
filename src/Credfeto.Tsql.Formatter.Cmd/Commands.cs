using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cocona;
using Credfeto.Tsql.Formatter.Cmd.LoggingExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Credfeto.Tsql.Formatter.Cmd;

[SuppressMessage(
    category: "Microsoft.Performance",
    checkId: "CA1812:AvoidUninstantiatedInternalClasses",
    Justification = "Instantiated by Cocona"
)]
[SuppressMessage(
    category: "ReSharper",
    checkId: "ClassNeverInstantiated.Global",
    Justification = "Instantiated by Cocona"
)]
internal sealed class Commands
{
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private readonly ITransactSqlFormatter _formatter;
    private readonly ILogger<Commands> _logger;

    [SuppressMessage(
        category: "FunFair.CodeAnalysis",
        checkId: "FFS0023: Use ILogger rather than ILogger<T>",
        Justification = "Needed in this case"
    )]
    public Commands(ITransactSqlFormatter formatter, ILogger<Commands> logger)
    {
        this._formatter = formatter;
        this._logger = logger;
    }

    [Command("format", Description = "Update all packages in all repositories")]
    [SuppressMessage(category: "ReSharper", checkId: "UnusedMember.Global", Justification = "Used by Cocona")]
    public async Task FormatFilesAsync(
        [Option(name: "work", ['w'], Description = "folder where to clone repositories")] string workFolder
    )
    {
        SqlScriptGeneratorOptions options = TSqlOptions.DefaultOptions;

        string[] sqlFiles = Directory.GetFiles(
            path: workFolder,
            searchPattern: "*.sql",
            searchOption: SearchOption.AllDirectories
        );

        foreach (string file in sqlFiles)
        {
            this._logger.StartingFile(file);

            string original = await File.ReadAllTextAsync(
                path: file,
                encoding: Encoding.UTF8,
                cancellationToken: this._cancellationToken
            );

            string formatted = await this._formatter.FormatAsync(
                source: original,
                options: options,
                cancellationToken: this._cancellationToken
            );

            if (StringComparer.Ordinal.Equals(x: original, y: formatted))
            {
                this._logger.NotChanged(file);
            }
            else
            {
                this._logger.Changed(file);
                await File.WriteAllTextAsync(
                    path: file,
                    contents: formatted,
                    encoding: Encoding.UTF8,
                    cancellationToken: this._cancellationToken
                );
            }
        }

        this.Done();
    }

    private void Done()
    {
        this._logger.LogCompleted();
    }
}
