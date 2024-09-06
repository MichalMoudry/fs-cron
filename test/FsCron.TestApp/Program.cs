using FsCron;
using FsCron.TestApp.Database;
using FsCron.TestApp.Services;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddMediator();
services.AddDbContext<PetStoreContext>();
var provider = services.BuildServiceProvider();

Console.WriteLine("Hello, World!");
var mediator = provider.GetRequiredService<IMediator>();

using var scheduler = new Scheduler();
scheduler.NewJob("* * * * *", Print);
//scheduler.NewAsyncJob("* * * * *", InsertTask);
//scheduler.NewAsyncJob("*/2 * * * *", SelectTask);
scheduler.Start();

Console.WriteLine("Press any key to exit...");
Console.ReadLine();

scheduler.Stop();
return;

void Print() => Console.WriteLine($"[{DateTimeOffset.Now}] Echo");

async Task InsertTask(CancellationToken token)
{
    Console.WriteLine($"[{DateTime.Now}] Start...");
    await mediator.Send(new PetInsertCommand("Cari"), token);
    Console.WriteLine($"[{DateTime.Now}] Executed...");
}

async Task SelectTask(CancellationToken token)
{
    Console.WriteLine($"[{DateTime.Now}] Pets:");
    var pets = await mediator.Send(
        new PetSelectCommand(),
        token
    );
    foreach (var pet in pets)
    {
        Console.WriteLine(pet);
    }
}
