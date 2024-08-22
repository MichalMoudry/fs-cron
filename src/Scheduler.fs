namespace FsCron

open System
open System.Collections.Generic
open System.Threading
open System.Threading.Tasks
open Cronos

[<Sealed>]
type Scheduler() =
    let jobs = List<JobDefinition>()

    /// Method for adding a new job into a scheduler.
    member this.NewJob (cronDef: string) (job: Action) =
        jobs.Add(JobDefinition(
            CronExpression.Parse(cronDef),
            job
        ))

    member this.NewAsyncJob (cronDef: string) (job: Task) =
        ()

    /// Method for initializing the scheduler and starting all its jobs.
    member this.Start isParallel =
        if jobs.Count = 0 then ()
        else
            ()

    member this.Stop() = ()
