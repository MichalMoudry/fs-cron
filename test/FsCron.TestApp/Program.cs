using FsCron;

Console.WriteLine("Hello, World!");

var scheduler = new Scheduler();
scheduler.NewJob("* * * * *", () =>
{
    Console.WriteLine($"[{DateTimeOffset.Now}] Echo");
});
scheduler.NewJob("* * * * *", () =>
{
    Console.WriteLine($"[{DateTimeOffset.Now}] Echo 2");
});
scheduler.Start(false);

Console.WriteLine("Press any key to exit...");
Console.ReadLine();

scheduler.Stop();