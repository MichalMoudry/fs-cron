namespace FsCron

open System
open System.Threading.Tasks
open Cronos

type internal IJobDefinition =
    abstract CurrentDate: DateTimeOffset
    abstract ExecuteAsync: unit -> Task
    abstract Execute: unit -> unit
    abstract IsAsync: bool

[<Sealed>]
type internal JobDefinition(
    cronExp: CronExpression,
    work: option<Task>,
    action: option<Action>) =
    let mutable date =
        cronExp.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local).Value

    interface IJobDefinition with
        member this.CurrentDate = date

        member this.ExecuteAsync() =
            task {
                return ()
            }

        member this.Execute() =
            match action with
            | Some method -> method.Invoke()
            | None -> ()
            let next = cronExp.GetNextOccurrence(date, TimeZoneInfo.Local)
            date <- if next.HasValue then next.Value else date

        member this.IsAsync = work.IsSome
