namespace FsCron

open System.Threading.Tasks
open Cronos
open System

type IJobDefinition =
    abstract CurrentDate: DateTimeOffset
    abstract Execute: unit -> Task<unit>

[<Sealed>]
type internal JobDefinition(cronExp: CronExpression, work: Task) =
    let mutable date =
        cronExp.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local).Value

    interface IJobDefinition with
        member this.CurrentDate = date
        member this.Execute() =
            task {
                do! work
                let next = cronExp.GetNextOccurrence(date, TimeZoneInfo.Local)
                date <- if next.HasValue then next.Value else date
            }

    new(cronExp: CronExpression, action: Action) =
        JobDefinition(cronExp, new Task(action))
