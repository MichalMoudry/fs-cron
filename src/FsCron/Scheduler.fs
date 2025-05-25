namespace FsCron

open System
open System.Collections.Generic
open System.Threading
open System.Threading.Tasks
open Cronos

/// A job scheduler that runs in either synchronous or asynchronous mode.
[<Sealed>]
type Scheduler(tzInfo: TimeZoneInfo) =
    let jobs = List<JobDefinition>()
    let tokenSource = new CancellationTokenSource()
    let mutable storage = option<IStorage>.None
    let maxIterationDuration = TimeSpan.FromMilliseconds(1000)
    let mutable isDisposed = false
    let mutable isRunning = false

    let startInternal() =
        let mutable startTimeStamp = DateTimeOffset.MinValue
        let cancellationToken = tokenSource.Token

        while not(cancellationToken.IsCancellationRequested) do
            startTimeStamp <- DateTimeOffset.Now

            for job in jobs do
                // TODO: Investigate retry with <= 0 condition
                if Calc.GetNextJobOccurrenceDiff(job.NextOccurrence) < 0 then
                    match job with
                    | :? AsyncJobDefinition as jobDef ->
                        jobDef.ExecuteAsync(cancellationToken)
                        |> Async.AwaitTask
                        |> Async.Start
                    | :? SyncJobDefinition as jobDef ->
                        ThreadPool.QueueUserWorkItem(fun i -> jobDef.Execute())
                        |> ignore
                    | _ -> failwith "Unknown job type"

            // DateTimeOffset.Now - startTimeStamp = how long the job enqueuing took
            let timeout =
                maxIterationDuration - (DateTimeOffset.Now - startTimeStamp)
            if timeout < TimeSpan.Zero then
                failwith "Iteration took too long"
            else
                Thread.Sleep(timeout)

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

    member this.NewJobFromExpr expr job =
        jobs.Add(SyncJobDefinition(expr, tzInfo, job))

    /// Adds a new asynchronous job to the scheduler.
    member this.NewAsyncJob cronExpr job =
        jobs.Add(AsyncJobDefinition(CronExpression.Parse(cronExpr), tzInfo, job))

    member this.NewAsyncJobFromExpr expr job =
        jobs.Add(AsyncJobDefinition(expr, tzInfo, job))

    member this.SetExternalStorage(externalStorage) =
        storage <- Some(externalStorage)

    /// Starts scheduler and blocks the current thread.
    member this.Start() =
        if not(isRunning) then
            startInternal()
            isRunning <- true

    /// Starts scheduler in an asynchronous/non-blocking manner in a
    /// separate background <seealso cref="Thread"/>.
    member this.StartAsync() =
        if not(isRunning) then
            Thread(startInternal, IsBackground = true).Start()
            isRunning <- true

    with override this.ToString() = $"Scheduler(NumberOfJobs = {jobs.Count}, Tz = {tzInfo.BaseUtcOffset}, IsRunning = {isRunning})"
