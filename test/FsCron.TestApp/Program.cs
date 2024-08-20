// See https://aka.ms/new-console-template for more information
using FsCron;

Console.WriteLine("Hello, World!");

var scheduler = new Scheduler();
scheduler.NewJob("* * * * *", () =>
{
    Console.WriteLine($"[{DateTimeOffset.Now}] Echo");
});
scheduler.Start();

Console.WriteLine("Press any key to exit...");
Console.ReadLine();