using System.Threading;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Credfeto.Tsql.Formatter;

public interface ITransactSqlFormatter
{
    public ValueTask<string> FormatAsync(
        string source,
        SqlScriptGeneratorOptions options,
        in CancellationToken cancellationToken
    );
}
