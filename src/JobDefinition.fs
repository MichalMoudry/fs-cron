namespace FsCron

open System.Threading
open Cronos
open System

type internal JobDefinition(cronExp: ICronExpression, work: Action) =
    let threadStart() =
        let nextOccurrence = cronExp.GetNextOccurrence(
            DateTimeOffset.Now,
            TimeZoneInfo.Local
        )
        work.Invoke()
    member this.CronExp with get() = cronExp
    member this.GetThreadStart() =
        ThreadStart(threadStart)
