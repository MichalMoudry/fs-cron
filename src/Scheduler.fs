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
        if jobs.Count = 0 then
            ()
        else
            while true do
                //let now = DateTimeOffset.Now
                for job in jobs do
                    if Math.Round((job.CurrentDate - DateTimeOffset.Now).TotalSeconds) = 0 then
                        //job.Execute() |> ignore
                        printfn "[Execute]"
                    else
                        printfn $"[{DateTimeOffset.Now}] => {job.CurrentDate}"
                    ()
                (*let now = DateTimeOffset.Now
                for job in jobs do
                    if job.CurrentDate = now then
                        job.Execute() |> Async.AwaitTask |> Async.RunSynchronously
                    else
                        printfn $"{now} | {job.CurrentDate} => {(now - job.CurrentDate).TotalSeconds}"*)
                Thread.Sleep(1000)

    member this.Stop() = ()
