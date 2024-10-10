using FsCron;
using FsCron.TestApp.Database;
using FsCron.TestApp.Domain;
using FsCron.TestApp.Services;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services
    .AddMediator()
    .AddDbContext<TestDbContext>()
    .AddLogging(builder => builder.AddConsole());
var provider = services.BuildServiceProvider();

Console.WriteLine("Hello, World!");
var mediator = provider.GetRequiredService<IMediator>();

using var scheduler = new Scheduler();
//scheduler.NewJob("* * * * *", Print);
scheduler.NewAsyncJob("* * * * *", PrintAsync);
//scheduler.NewAsyncJob("*/2 * * * *", SelectTask);
scheduler.Start();

Console.WriteLine("Press any key to exit...");
Console.ReadLine();

scheduler.Stop();
return;

/*void Print()
{
    Console.WriteLine($"[{DateTimeOffset.Now}] Echo 1");
    Thread.Sleep(2000);
    Console.WriteLine($"[{DateTimeOffset.Now}] Echo 2");
}*/

async Task PrintAsync(CancellationToken token)
{
    var start = DateTimeOffset.Now;
    await Task.Delay(2000, token);
    var end = DateTimeOffset.Now;

    await mediator.Send(
        new InsertJobResultCommand(JobType.PrintJob, start, end),
        token
    );
}
