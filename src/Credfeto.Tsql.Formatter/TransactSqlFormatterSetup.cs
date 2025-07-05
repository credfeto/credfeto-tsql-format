using Credfeto.Tsql.Formatter.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Credfeto.Tsql.Formatter;

public static class TransactSqlFormatterSetup
{
    public static IServiceCollection AddTransactSqlFormatting(this IServiceCollection services)
    {
        return services.AddSingleton<ITransactSqlFormatter, TransactSqlFormatter>();
    }
}
