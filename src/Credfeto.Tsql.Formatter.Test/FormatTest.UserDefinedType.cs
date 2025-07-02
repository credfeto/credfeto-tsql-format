using System.Threading.Tasks;
using Xunit;

namespace Credfeto.Tsql.Formatter.Test;

public sealed partial class FormatTest
{
    [Fact]
    public async Task FormatUserDefinedTypeAsync()
    {
        const string source = "SET QUOTED_IDENTIFIER ON\nGO\n\nSET ANSI_NULLS ON\nGO\n\nCREATE TYPE [Accounts].[AccountConfigType]\nFROM NVARCHAR(MAX)\nGO\n\n\n";
        const string expected = "SET QUOTED_IDENTIFIER ON;\n\n\nGO\nSET ANSI_NULLS ON;\n\n\nGO\nCREATE TYPE [Accounts].[AccountConfigType]\n    FROM NVARCHAR (MAX);\n\n\nGO\n\n\n";

        string result = await TsqlFormatting.FormatAsync(source: source, options: TSqlOptions.DefaultOptions, this.CancellationToken());

        this.CheckResults(expected: expected, result: result);
    }
}