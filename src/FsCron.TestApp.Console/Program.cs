using FsCron;
using FsCron.Monitor;
using FsCron.TestApp.Console;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");

using var source = new CancellationTokenSource();

using var loggerFactory = LoggerFactory.Create(
    builder => builder.AddConsole()
);
var logger = loggerFactory.CreateLogger<Program>();

try
{
    var scheduler = new Scheduler(source.Token);

    scheduler.AddMonitoring(
        new StorageSettings(
            StorageType.RemoteCache,
            "127.0.0.1:6379,password=AxKZn7WuI.2dB6dp5|1z,abortConnect=false"
        )
    );

    scheduler.NewAsyncJob(
        "* * * * *",
        PrintAsync,
        TimeZoneInfo.Local
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
