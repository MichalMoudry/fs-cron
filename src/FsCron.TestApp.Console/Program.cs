using FsCron;

Console.WriteLine("Hello, World!");

/*using var loggerFactory = LoggerFactory.Create(
    builder => builder.AddConsole()
);
var logger = loggerFactory.CreateLogger<Program>();*/

using var scheduler = new Scheduler(TimeZoneInfo.Local);
scheduler.NewAsyncJob(
    "* * * * *",
    async token =>
    {
        Console.WriteLine($"[{DateTimeOffset.Now}] Test print");
        await Task.Delay(500, token).ConfigureAwait(false);
        throw new InvalidOperationException("test exception");
    }
);

Console.WriteLine("Starting scheduler");
scheduler.Start();
