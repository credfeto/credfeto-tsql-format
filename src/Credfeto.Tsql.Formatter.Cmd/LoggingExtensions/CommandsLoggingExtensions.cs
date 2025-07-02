using Microsoft.Extensions.Logging;

namespace Credfeto.Tsql.Formatter.Cmd.LoggingExtensions;

internal static partial class CommandsLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Done")]
    public static partial void LogCompleted(this ILogger<Commands> logger);
}