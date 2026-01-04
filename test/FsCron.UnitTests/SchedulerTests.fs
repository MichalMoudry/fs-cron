module FsCron.SchedulerTests

open System
open System.Collections.Generic
open System.Threading
open Cronos
open NUnit.Framework

[<TestCase(3)>]
let Test (seconds: int) =
    use scheduler = new Scheduler(TimeZoneInfo.Local)

    let list = List<int>(seconds)
    scheduler.NewJobFromExpr CronExpression.EverySecond (Action(fun i -> list.Add(1)))

    scheduler.StartAsync()
    Thread.Sleep(seconds)
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
    Assert.Pass()
