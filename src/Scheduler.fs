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

    let tokenSource =
        match cancellationToken.HasValue with
        | true -> None
        | false -> Some(new CancellationTokenSource())

    let token =
        match cancellationToken.HasValue with
        | true -> cancellationToken.Value
        | false ->
            if tokenSource.IsSome then
                tokenSource.Value.Token
            else
                failwith "uninitialized token source"

    let startInternal() =
        let mutable now = DateTimeOffset.MinValue
        let maxIterationDuration = TimeSpan.FromSeconds(1)
        while true do
            now <- DateTimeOffset.Now
            for job in jobs do
                if Math.Round((job.CurrentDate - now).TotalSeconds) = 0 then
                    if job.IsAsync then
                        job.ExecuteAsync(token)
                        |> Async.AwaitTask
                        |> Async.Start
                    else
                        job.Execute()
            Thread.Sleep(maxIterationDuration - (DateTimeOffset.Now - now))

    interface IDisposable with
        member this.Dispose() =
            if tokenSource.IsSome then
                tokenSource.Value.Dispose()

    member this.Start () =
        Thread(startInternal, IsBackground = true).Start()

    member this.NewJob (cronDef: string) (job: Action) =
        jobs.Add(JobDefinition(
            CronExpression.Parse(cronDef),
            None,
            Some(job)
        ))

    member this.NewAsyncJob (cronDef: string) (job: Func<CancellationToken, Task>) =
        jobs.Add(JobDefinition(
            CronExpression.Parse(cronDef),
            Some(job),
            None
        ))

    /// Method for stopping the scheduler. Also, calls Dispose() method.
    member this.Stop() =
        tokenSource.Value.Cancel()
        (this :> IDisposable).Dispose()
