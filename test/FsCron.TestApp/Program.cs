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
//scheduler.NewJob("* * * * *", Print);
scheduler.NewAsyncJob("* * * * *", Wait);
scheduler.Start();

Console.WriteLine("Press any key to exit...");
Console.ReadLine();

scheduler.Stop();
return;

//void Print() => Console.WriteLine($"[{DateTimeOffset.Now}] Echo");

async Task Wait()
{
    Console.WriteLine($"[{DateTime.Now}] Start...");
    await mediator.Send(new PetInsertCommand("Cari"));
    await Task.Delay(2500);
    Console.WriteLine($"[{DateTime.Now}] Executed...");
}
