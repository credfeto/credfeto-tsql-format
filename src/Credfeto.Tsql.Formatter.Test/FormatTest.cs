using FunFair.Test.Common;
using Xunit;

namespace Credfeto.Tsql.Formatter.Test;

public sealed partial class FormatTest : LoggingTestBase
{
    public FormatTest(ITestOutputHelper output)
        : base(output)
    {
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