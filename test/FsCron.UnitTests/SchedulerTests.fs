module FsCron.SchedulerTests

open System
open System.Collections.Generic
open System.Diagnostics
open System.Threading.Tasks
open Cronos
open NUnit.Framework

[<TestCase(5)>]
let Test (seconds: int) =
    use scheduler = new Scheduler(TimeZoneInfo.Local)

    let list = List<int>(seconds)
    scheduler.NewJobFromExpr CronExpression.EverySecond (Action(fun i -> list.Add(1)))

    (*let sw = Stopwatch.StartNew()
    scheduler.StartAsync()
    while list.Count <> seconds do ()
    sw.Stop()
    Assert.That(sw.Elapsed.Seconds, Is.EqualTo(seconds))*)

    let sw = Stopwatch.StartNew()
    scheduler.StartAsync()

    Task.Delay(TimeSpan.FromSeconds(int64(seconds)))
    |> Async.AwaitTask
    |> Async.RunSynchronously
    sw.Stop()

    Assert.That(sw.Elapsed.Seconds, Is.EqualTo(seconds))
    Assert.That(list, Has.Count.EqualTo(seconds))

    (*for i in 0..seconds do
        let current_sec = i + 1
        Assert.That(
            list,
            Has.Count.EqualTo(current_sec).After(current_sec * 1000)
        )*)

    (*let numberOfSeconds = 10
    use scheduler = new Scheduler(TimeZoneInfo.Local)

    let list = List<int>(numberOfSeconds)
    scheduler.NewJobFromExpr
        CronExpression.EverySecond
        (Action(fun _ -> list.Add(1)))

    scheduler.StartAsync()
    let sw = Stopwatch.StartNew()
    while list.Count < 10 do Thread.Sleep(10)
    sw.Stop()

    Assert.That(sw.Elapsed.TotalSeconds, Is.EqualTo(numberOfSeconds))*)
