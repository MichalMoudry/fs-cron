namespace FsCron

open System
open Cronos

[<AbstractClass>]
type JobDefinition(cronExpr: CronExpression, tzInfo: TimeZoneInfo) =
    member this.CronExpression with get() = cronExpr
    member this.NextOccurrence with get() =
        cronExpr.GetNextOccurrence(DateTimeOffset.Now, tzInfo)
