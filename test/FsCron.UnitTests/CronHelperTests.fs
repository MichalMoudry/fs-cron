module FsCron.UnitTests.CronHelperTests

open System
open FsCron
open NUnit.Framework

[<Test>]
let TestSimpleInput() =
    CronHelper.NextRun "* * * * 0" DateTimeOffset.Now
    ()