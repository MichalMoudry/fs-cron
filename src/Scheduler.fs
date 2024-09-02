namespace FsCron

open System
open System.Collections.Generic
open System.Threading
open System.Threading.Tasks
open Cronos

/// A job scheduler that runs in a separate background thread.
[<Sealed>]
type Scheduler() =
    let jobs = List<IJobDefinition>()

    let startInternal() =
        while true do
            let now = DateTimeOffset.Now
            for job in jobs do
                if Math.Round((job.CurrentDate - now).TotalSeconds) = 0 then
                    if job.IsAsync then
                        //ThreadPool.QueueUserWorkItem(job.ExecuteAsync)
                        printfn "[Async execute]"
                    else
                        printfn "[Action execute]"
                        job.Execute()
                else
                    printfn $"[{now}] => {job.CurrentDate}"
            Thread.Sleep(1000)

    member this.Start() =
        Thread(startInternal, IsBackground = true).Start()

    member this.NewJob (cronDef: string) (job: Action) =
        jobs.Add(JobDefinition(
            CronExpression.Parse(cronDef),
            None,
            Some(job)
        ))

    member this.NewAsyncJob (cronDef: string) (job: Func<Task>) =
        jobs.Add(JobDefinition(
            CronExpression.Parse(cronDef),
            Some(job),
            None
        ))

    member this.Stop() = ()
