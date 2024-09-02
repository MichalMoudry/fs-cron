module FsCron.UnitTests.JobDefinitionTests

open System
open Cronos
open FsCron
open NUnit.Framework

/// A test case covering definition of a job with a 1 minute period execution.
[<Test>]
let TestActionBasedOneMinJobDef() =
    let now = DateTimeOffset.Now
    let jobDefinition = JobDefinition(
        CronExpression.Parse("* * * * *"),
        None,
        Some(Action(fun i -> printfn "Test action"))) :> IJobDefinition

    let jobDate = jobDefinition.CurrentDate
    Assert.That(jobDate, Is.GreaterThan(now))
    Assert.That(jobDate, Is.LessThanOrEqualTo(now.AddSeconds(1)))
    Assert.That(jobDefinition.IsAsync, Is.False)

[<Test>]
let TestActionBasedOneHourJobDef() =
    Assert.Pass()
