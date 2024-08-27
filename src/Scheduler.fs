namespace FsCron

open System
open System.Collections.Generic
open System.Threading
open System.Threading.Tasks
open Cronos
open Microsoft.FSharp.Control

[<Sealed>]
type Scheduler() =
    let jobs = List<IJobDefinition>()

    /// Method for adding a new job into a scheduler.
    member this.NewJob (cronDef: string) (job: Action) =
        jobs.Add(JobDefinition(
            CronExpression.Parse(cronDef),
            job
        ))

    member this.NewAsyncJob (cronDef: string) (job: Task) =
        jobs.Add(JobDefinition(
            CronExpression.Parse(cronDef),
            job
        ))

    /// Method for initializing the scheduler and starting all its jobs.
    member this.Start isParallel =
        task {
            if jobs.Count = 0 then
                ()
            else
                while true do
                    let now = DateTimeOffset.Now
                    for job in jobs do
                        if Math.Round((job.CurrentDate - now).TotalSeconds) = 0 then
                            printfn "[Starting execute]"
                            do! job.Execute()
                            printfn "[Executed]"
                        else
                            printfn $"[{DateTimeOffset.Now}] => {job.CurrentDate}"
                    Thread.Sleep(1000)
        }

    member this.Stop() = ()
