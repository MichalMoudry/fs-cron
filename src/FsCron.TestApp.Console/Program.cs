using FsCron;

Console.WriteLine("Hello, World!");

using var source = new CancellationTokenSource();
var scheduler = new Scheduler(source.Token);

scheduler.NewAsyncJob(
    "* * * * *",
    PrintAsync,
    TimeZoneInfo.Local
);

scheduler.Start();
return;

async Task PrintAsync(CancellationToken token)
{
    await Task.Delay(1000, token);
    Console.WriteLine($"[{DateTimeOffset.Now}] Test print");
}