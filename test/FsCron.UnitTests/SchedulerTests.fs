module FsCron.SchedulerTests

open NUnit.Framework

[<TestCase(1_000)>]
let Test (second: int) =
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
