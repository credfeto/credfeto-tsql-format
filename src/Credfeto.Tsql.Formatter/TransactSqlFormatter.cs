using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Credfeto.Tsql.Formatter;

public sealed class TransactSqlFormatter : ITransactSqlFormatter
{
    public ValueTask<string> FormatAsync(string source, SqlScriptGeneratorOptions options, in CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(source))
        {
            return ValueTask.FromResult(source);
        }

        if (!TryParse(text: source, out TSqlFragment? fragment))
        {
            return ValueTask.FromResult(source);
        }


        cancellationToken.ThrowIfCancellationRequested();

        Sql170ScriptGenerator generator = new(options);

        generator.GenerateScript(scriptFragment: fragment, out string? formattedSql);

        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(formattedSql))
        {
            return ValueTask.FromResult(source);
        }

        string[] splitResults = formattedSql.Split('\n');

        int pos = splitResults.Length - 1;

        while (pos >= 0 && string.IsNullOrEmpty(splitResults[pos]))
        {
            --pos;
        }

        if (pos == 0)
        {
            return ValueTask.FromResult(source);
        }

        if (StringComparer.Ordinal.Equals(splitResults[pos], "GO"))
        {
            return ValueTask.FromResult(formattedSql);
        }

        return ValueTask.FromResult(formattedSql + "\nGO\n\n\n");
    }

    private static bool TryParse(string text, [NotNullWhen(true)] out TSqlFragment? fragment)
    {
        try
        {
            TSql170Parser parser = new(initialQuotedIdentifiers: true, engineType: SqlEngineType.All);

            using (StringReader reader = new(text))
            {
                fragment = parser.Parse(input: reader, out IList<ParseError> errors);

                return errors.Count == 0;
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception.Message);
            fragment = null;

            return false;
        }
    }
}