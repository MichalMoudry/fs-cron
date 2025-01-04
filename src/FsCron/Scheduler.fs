namespace FsCron

open System
open System.Collections.Generic
open System.Threading
open System.Threading.Tasks
open Cronos

/// A job scheduler that runs in either synchronous or asynchronous mode.
[<Sealed>]
type Scheduler(tzInfo: TimeZoneInfo) =
    let mutable isDisposed = false
    let mutable isRunning = false
    let jobs = List<JobDefinition>()
    let maxIterationDuration = TimeSpan.FromSeconds(int64(1))
    let tokenSource = new CancellationTokenSource()

    let startInternal() =
        let mutable startTimeStamp = DateTimeOffset.MinValue
        let cancellationToken = tokenSource.Token

        while not(cancellationToken.IsCancellationRequested) do
            startTimeStamp <- DateTimeOffset.Now

            for job in jobs do
                // TODO: Investigate retry with <= 0 condition
                let diff = Math.Round(job.NextOccurrenceDiff.TotalSeconds) 
                if diff = 0 then
                    match job with
                    | :? AsyncJobDefinition as jobDef ->
                        jobDef.ExecuteAsync(cancellationToken)
                        |> Async.AwaitTask
                        |> Async.Start
                    | :? SyncJobDefinition as jobDef ->
                        if ThreadPool.QueueUserWorkItem(fun i -> jobDef.Execute()) then
                            ()
                        else
                            failwith "Sync job was not queued on thread pool"
                    | _ -> failwith "Unknown job type"

            Thread.Sleep(maxIterationDuration - (DateTimeOffset.Now - startTimeStamp))

    let Dispose disposing =
        if not(isDisposed) then
            if disposing then
                tokenSource.Dispose()
            isDisposed <- true

    interface IDisposable with
        member this.Dispose() =
            tokenSource.Cancel()
            Dispose(true)
            GC.SuppressFinalize(this)

    interface IAsyncDisposable with
        member this.DisposeAsync() =
            task {
                do! tokenSource.CancelAsync()
                Dispose(true)
                GC.SuppressFinalize(this)
            } |> ValueTask

    /// Adds a new synchronous job to the scheduler.
    member this.NewJob cronExpr job =
        jobs.Add(SyncJobDefinition(CronExpression.Parse(cronExpr), tzInfo, job))

    /// Adds a new asynchronous job to the scheduler.
    member this.NewAsyncJob cronExpr job =
        jobs.Add(AsyncJobDefinition(CronExpression.Parse(cronExpr), tzInfo, job))

    /// Starts scheduler and blocks the current thread.
    member this.Start() =
        if not(isRunning) then
            startInternal()
            isRunning <- true
        else ()

    /// Starts scheduler in an asynchronous manner in a
    /// separate background <seealso cref="Thread"/>.
    member this.StartAsync() =
        if not(isRunning) then
            Thread(startInternal, IsBackground = true).Start()
            isRunning <- true
        else ()
