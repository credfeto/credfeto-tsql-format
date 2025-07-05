using System.Threading.Tasks;
using Credfeto.Tsql.Formatter.Services;
using FunFair.Test.Common;
using Xunit;

namespace Credfeto.Tsql.Formatter.Test;

public sealed partial class FormatTest : LoggingTestBase
{
    private readonly ITransactSqlFormatter _formatter;

    public FormatTest(ITestOutputHelper output)
        : base(output)
    {
        this._formatter = new TransactSqlFormatter();
    }

    private async Task ReformatAsync(string source, string expected)
    {
        string result = await this._formatter.FormatAsync(
            source: source,
            options: TSqlOptions.DefaultOptions,
            this.CancellationToken()
        );

        this.CheckResults(expected: expected, result: result);
    }

    private void CheckResults(string expected, string result)
    {
        this.Output.WriteLine("Expected:");
        this.Output.WriteLine(expected);

        this.Output.WriteLine("Actual:");
        this.Output.WriteLine(result);
        Assert.Equal(expected: expected, actual: result);
    }
}
