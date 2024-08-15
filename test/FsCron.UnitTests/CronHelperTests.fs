module FsCron.UnitTests.CronHelperTests

open FsCron
open NUnit.Framework

[<Test>]
let TestSimpleInput() =
    CronHelper.ParseCron("* * * * *")
    ()