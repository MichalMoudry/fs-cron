using Microsoft.Extensions.Logging;

namespace FsCron.TestApp.Console;

internal static partial class Log
{
    [LoggerMessage(
        LogLevel.Error,
        Message = "Error occurred when creating and starting scheduler")]
    public static partial void LogSchedulerErr(ILogger logger, Exception ex);

    [LoggerMessage(
        LogLevel.Information,
        Message = "[{Now}] Test print - {Id}")]
    public static partial void LogTestPrint(
        ILogger logger,
        DateTimeOffset now,
        Guid id
    );
}