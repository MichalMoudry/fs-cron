using FsCron;

Console.WriteLine("Hello, World!");

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
    await Task.Delay(5000);
    Console.WriteLine($"[{DateTime.Now}] Executed...");
}
