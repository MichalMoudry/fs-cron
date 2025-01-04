using FsCron;
using FsCron.TestApp.Console;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");

using var loggerFactory = LoggerFactory.Create(
    builder => builder.AddConsole()
);
var logger = loggerFactory.CreateLogger<Program>();

try
{
    await using var scheduler = new Scheduler(TimeZoneInfo.Local);

    scheduler.NewAsyncJob(
        "* * * * *",
        PrintAsync
    );

    Console.WriteLine("Starting scheduler");
    scheduler.Start();
}
catch (Exception e)
{
    Log.LogSchedulerErr(logger, e);
}

return;

async Task PrintAsync(CancellationToken token)
{
    Console.WriteLine($"[{DateTimeOffset.Now}] Test print");
    await Task.Delay(1000, token);
}
