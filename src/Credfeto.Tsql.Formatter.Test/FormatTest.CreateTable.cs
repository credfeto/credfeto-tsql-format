using System.Threading.Tasks;
using Xunit;

namespace Credfeto.Tsql.Formatter.Test;

public sealed partial class FormatTest
{
    [Fact]
    public Task FormatCreateTableAsync()
    {
        const string source = "CREATE TABLE [Utils].[Dates] (\n    [StartDate] [Config].[DateTimeType] NOT NULL,\n    [EndDate] [Config].[DateTimeType] NOT NULL\n    ) ON [PRIMARY]\nGO\n\nALTER TABLE [Utils].[Dates] ADD CONSTRAINT [PK_Dates] PRIMARY KEY ([StartDate] DESC) ON [PRIMARY]\nGO\n\nALTER TABLE [Utils].[Dates] ADD CONSTRAINT [IUX_Dates] UNIQUE ([EndDate] DESC) ON [PRIMARY]\nGO\n\nGRANT SELECT\n    ON [Utils].[Dates]\n    TO Reports\nGO\n\n\n";
        const string expected = "CREATE TABLE [Utils].[Dates]\n(\n    [StartDate] [Config].[DateTimeType] NOT NULL,\n    [EndDate]   [Config].[DateTimeType] NOT NULL\n) ON [PRIMARY];\n\n\nGO\nALTER TABLE [Utils].[Dates]\n    ADD CONSTRAINT [PK_Dates] PRIMARY KEY ([StartDate] DESC) ON [PRIMARY];\n\n\nGO\nALTER TABLE [Utils].[Dates]\n    ADD CONSTRAINT [IUX_Dates] UNIQUE ([EndDate] DESC) ON [PRIMARY];\n\n\nGO\nGRANT SELECT\n    ON [Utils].[Dates] TO Reports;\n\n\nGO\n\n\n";

        return this.ReformatAsync(source: source, expected: expected);
    }


}