using Microsoft.Extensions.Logging;

namespace FsCron.TestApp.Console;

internal static partial class Log
{
    [LoggerMessage(
        LogLevel.Error,
        Message = "Error occurred when creating and starting scheduler")]
    public static partial void LogSchedulerErr(ILogger logger, Exception ex);
}