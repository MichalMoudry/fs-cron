module FsCron.SchedulerTests

open System
open System.Collections.Generic
open System.Threading
open Cronos
open FsCron
open NUnit.Framework

[<TestCase(1_000)>]
let Test (second: int) =
    use scheduler = new Scheduler(TimeZoneInfo.Local)
    let list = List<int>()
    scheduler.NewJobFromExpr
        CronExpression.EverySecond
        (Action(fun _ -> list.Add(1)))

    printfn "Starting scheduler in a separate thread"
    scheduler.StartAsync()

    printfn "Sleep thread for 60 seconds"
    let numberOfSeconds = 60
    Thread.Sleep(second * numberOfSeconds)

    Assert.That(list, Has.Count.EqualTo(numberOfSeconds))
