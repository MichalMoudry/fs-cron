namespace FsCron

open System
open System.Collections.Generic
open System.Runtime.InteropServices
open System.Threading
open System.Threading.Tasks
open Cronos

/// A job scheduler that runs in a separate background thread.
[<Sealed>]
type Scheduler([<Optional>] cancellationToken: Nullable<CancellationToken>) =
    let jobs = List<IJobDefinition>()
    let tokenSource = new CancellationTokenSource()
    let token =
        match cancellationToken.HasValue with
        | true -> cancellationToken.Value
        | false -> tokenSource.Token

    let startInternal() =
        while true do
            let now = DateTimeOffset.Now
            for job in jobs do
                if Math.Round((job.CurrentDate - now).TotalSeconds) = 0 then
                    if job.IsAsync then
                        job.ExecuteAsync() |> Async.AwaitTask |> Async.Start
                    else
                        job.Execute()
            Thread.Sleep(1000)

    interface IDisposable with
        member this.Dispose() =
            tokenSource.Dispose()

    member this.Start () =
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

    member this.Stop() =
        ()
