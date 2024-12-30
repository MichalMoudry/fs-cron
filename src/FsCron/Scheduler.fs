namespace FsCron

open System
open System.Collections.Generic
open System.Threading
open Cronos
open FsCron.Monitor

/// A job scheduler that runs in either synchronous or asynchronous mode.
[<Sealed>]
type Scheduler(cancellationToken: CancellationToken) =
    let jobs = List<JobDefinition>()
    let maxIterationDuration = TimeSpan.FromSeconds(int64(1))

    let startInternal() =
        let mutable startTimeStamp = DateTimeOffset.MinValue

        while not(cancellationToken.IsCancellationRequested) do
            startTimeStamp <- DateTimeOffset.Now

            for job in jobs do
                // TODO: Investigate retry with <= 0 condition
                if Math.Round(job.NextOccurrenceDiff.TotalSeconds) = 0 then
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

    /// Adds a new synchronous job to the scheduler.
    member this.NewJob cronExpr tzInfo job =
        jobs.Add(SyncJobDefinition(CronExpression.Parse(cronExpr), tzInfo, job))

    /// Adds a new asynchronous job to the scheduler.
    member this.NewAsyncJob cronExpr job tzInfo =
        jobs.Add(AsyncJobDefinition(CronExpression.Parse(cronExpr), tzInfo, job))

    /// Method for adding/enabling of monitoring of jobs.
    member this.AddMonitoring(settings: StorageSettings) =
        match settings.Type with
        | StorageType.RemoteCache -> RemoteCache.Connect(settings.ConnectionString)
        | _ -> failwith "Incorrect storage settings"

    /// Starts scheduler and blocks the current thread.
    member this.Start() = startInternal()

    /// Starts scheduler in an asynchronous manner in a
    /// separate background <seealso cref="Thread"/>.
    member this.StartAsync() = Thread(startInternal, IsBackground = true).Start()
