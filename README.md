# FsCron
A library for periodically running tasks. This library was built on top of [Cronos](https://github.com/HangfireIO/Cronos).

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
Internally, `Scheduler` class has two methods `Start()` and `StartAsync()`. Start() executes startInternal() method directly and StartAsync() treats startInternal() as a ThreadStart delegate for a background Thread.

## Types of jobs
- Async job
- Sync job

## Job cancellation
