namespace FsCron

open System
open System.Collections.Generic
open Cronos

[<Sealed>]
type Scheduler() =
    let jobs = List<JobDefinition>()

    let startBlocking() = ()

    let startParallel() = ()

    member this.NewJob (cronDef: string) (job: Action) =
        jobs.Add(JobDefinition(
            CronExpression.Parse(cronDef),
            job
        ))

    member this.Start isParallel =
        match isParallel with
        | true -> startBlocking()
        | false -> startParallel()
        (*let thread = Thread(ThreadStart(startInternal))
        thread.IsBackground <- true
        thread.Start()*)

    member this.Stop() = ()
