module FsCron.UnitTests

open System
open Cronos
open NUnit.Framework

[<Test>]
let TestSyncJobDefinitionInit() =
    let job = SyncJobDefinition(
        CronExpression.Parse("* * * * *"),
        TimeZoneInfo.Utc,
        fun i -> printfn "test value"
    )
    let now = DateTimeOffset.UtcNow

    // +1 because the cron expr. is * * * * *
    let expectedMinute = now.Minute + 1
    Assert.That(
        job.NextOccurrence,
        Is.EqualTo(
            DateTimeOffset(
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                expectedMinute,
                0,
                now.Offset
            )
        )
    )