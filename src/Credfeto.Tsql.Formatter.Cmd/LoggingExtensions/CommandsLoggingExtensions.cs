using Microsoft.Extensions.Logging;

namespace Credfeto.Tsql.Formatter.Cmd.LoggingExtensions;

internal static partial class CommandsLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Done")]
    public static partial void LogCompleted(this ILogger<Commands> logger);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Starting {fileName}")]
    public static partial void StartingFile(this ILogger<Commands> logger, string fileName);

    [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "File {fileName} was changed")]
    public static partial void Changed(this ILogger<Commands> logger, string fileName);

    [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "File {fileName} was not changed")]
    public static partial void NotChanged(this ILogger<Commands> logger, string fileName);
}