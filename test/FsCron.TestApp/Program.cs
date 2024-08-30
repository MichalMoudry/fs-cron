using FsCron;

Console.WriteLine("Hello, World!");

var scheduler = new Scheduler();
scheduler.NewJob("* * * * *", Print);
//scheduler.NewAsyncJob("* * * * *", Wait());
scheduler.Start();

Console.WriteLine("Press any key to exit...");
Console.ReadLine();

scheduler.Stop();
return;

void Print() => Console.WriteLine($"[{DateTimeOffset.Now}] Echo");

Task Wait() => Task.Delay(5000);
