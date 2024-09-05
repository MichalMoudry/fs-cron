namespace FsCron

open System
open System.Threading
open System.Threading.Tasks
open Cronos

type internal IJobDefinition =
    abstract CurrentDate: DateTimeOffset
    abstract ExecuteAsync: CancellationToken -> Task
    abstract Execute: unit -> unit
    abstract IsAsync: bool

[<Sealed>]
type internal JobDefinition(
    cronExp: CronExpression,
    work: option<Func<CancellationToken, Task>>,
    action: option<Action>) =
    let mutable date =
        cronExp.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local).Value

    interface IJobDefinition with
        member this.CurrentDate = date

        member this.ExecuteAsync(token) =
            task {
                do! work.Value.Invoke(token)
                let next = cronExp.GetNextOccurrence(date, TimeZoneInfo.Local)
                date <- if next.HasValue then next.Value else date
            }

        member this.Execute() =
            match action with
            | Some method -> method.Invoke()
            | None -> ()
            let next = cronExp.GetNextOccurrence(date, TimeZoneInfo.Local)
            date <- if next.HasValue then next.Value else date

        member this.IsAsync = work.IsSome
