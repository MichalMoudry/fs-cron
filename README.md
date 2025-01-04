# FsCron
A .NET library for periodically running tasks. This library was built on top of [Cronos](https://github.com/HangfireIO/Cronos).

## Basic usage
```csharp
await using var scheduler = new Scheduler(TimeZoneInfo.Local);

scheduler.NewAsyncJob(
    "* * * * *",
    async token =>
    {
        Console.WriteLine($"[{DateTimeOffset.Now}] Test print");
        await Task.Delay(1000, token);
    }
);

scheduler.Start();
```

### Async usage
Asynchronous usage is about running the scheduler in a separate thread.

```csharp
await using var scheduler = new Scheduler(TimeZoneInfo.Local);

scheduler.NewAsyncJob(
    "* * * * *",
    async token =>
    {
        Console.WriteLine($"[{DateTimeOffset.Now}] Test print");
        await Task.Delay(1000, token);
    }
);

scheduler.StartAsync();
```
Internally, `Scheduler` class has two methods `Start()` and `StartAsync()`. `Start()` executes startInternal() method directly and `StartAsync()` treats startInternal() as a ThreadStart delegate for a background [Thread](https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread?view=net-9.0 "Link to Thread class documentation").

## Types of jobs
- Async job - A [Func](https://learn.microsoft.com/en-us/dotnet/api/system.func-2?view=net-9.0) delegate that has one parameter (cancellation token) and returns a Task. The returned task is not awaited, but rather started on a thread pool.
- Sync job: An [Action](https://learn.microsoft.com/en-us/dotnet/api/system.action?view=net-9.0 "Link to Action delegate documentation") delegate that is periodically queued on the thread pool, so that it is not blocking scheduler's own execution.

## Job cancellation
`Scheduler` class realizes IDisposable (and IAsyncDisposable) interface. Also, the class internally stores a CancellationTokenSource which is cancelled and disposed along side the scheduler itself.
