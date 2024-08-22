namespace FsCron

open System.Threading.Tasks
open Cronos
open System

type IJobDefinition =
    abstract CurrentDate: DateTimeOffset

[<Sealed>]
type internal JobDefinition(cronExp: CronExpression, work: Action) =
    let mutable date = DateTimeOffset.Now
    member this.CurrentDate with get() = date

[<Sealed>]
type internal AsyncJobDefinition(cronExp: CronExpression, work: Task) =
    let mutable date = DateTimeOffset.Now
    member this.CurrentDate with get() = date
    member this.Exec() =
        task {
            do! work
            date <- cronExp.GetNextOccurrence(date, TimeZoneInfo.Local).Value
        }
