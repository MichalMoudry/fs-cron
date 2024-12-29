namespace FsCron

open System
open Cronos

/// A base class for both sync and async job definitions.
[<AbstractClass>]
type JobDefinition(cronExpr: CronExpression, tzInfo: TimeZoneInfo) =
    let mutable currentDate =
        cronExpr.GetNextOccurrence(DateTimeOffset.Now, tzInfo)
    member this.CronExpression with get() = cronExpr
    member this.NextOccurrence with get() =
        match currentDate.HasValue with
        | true -> currentDate.Value
        | false -> DateTimeOffset.MinValue
    /// A diff between now and job's supposed run time.
    member this.NextOccurrenceDiff with get() =
        this.NextOccurrence - DateTimeOffset.Now
    member this.UpdateNextOccurrence() =
        currentDate <- cronExpr.GetNextOccurrence(DateTimeOffset.Now, tzInfo)
