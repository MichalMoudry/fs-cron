// See https://aka.ms/new-console-template for more information
using FsCron;

Console.WriteLine("Hello, World!");

var scheduler = new Scheduler();
scheduler.Start();

Console.WriteLine("Press any key to exit...");
Console.ReadLine();