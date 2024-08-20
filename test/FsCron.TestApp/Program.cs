using FsCron;

Console.WriteLine("Hello, World!");

var scheduler = new Scheduler();
scheduler.NewJob("* * * * *", () =>
{
    Console.WriteLine($"[{DateTimeOffset.Now}] Echo");
});
scheduler.Start(false);

Console.WriteLine("Press any key to exit...");
Console.ReadLine();