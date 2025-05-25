using Cronos;
using FsCron;
using FsCron.TestApp.Console;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");

using var loggerFactory = LoggerFactory.Create(
    builder => builder.AddConsole()
);
var logger = loggerFactory.CreateLogger<Program>();

using var scheduler = new Scheduler(TimeZoneInfo.Local);
/*scheduler.NewAsyncJob(
    "* * * * *",
    async token =>
    {
        Console.WriteLine($"[{DateTimeOffset.Now}] Test print");
        await Task.Delay(500, token).ConfigureAwait(false);
        throw new InvalidOperationException("test exception");
    }
);*/
scheduler.NewAsyncJobFromExpr(
    CronExpression.EverySecond,
    _ =>
    {
        Log.LogTestPrint(logger, DateTimeOffset.Now, Guid.CreateVersion7());
        return Task.CompletedTask;
    }
);

Console.WriteLine("Starting scheduler");
scheduler.Start();
