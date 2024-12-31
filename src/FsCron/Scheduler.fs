namespace FsCron

open System
open System.Collections.Generic
open System.Threading
open Cronos
open FsCron.Monitoring

/// A job scheduler that runs in either synchronous or asynchronous mode.
[<Sealed>]
type Scheduler(cancellationToken: CancellationToken) =
    let mutable isRunning = false
    let mutable areJobsMonitored = false
    let jobs = List<JobDefinition>()
    let maxIterationDuration = TimeSpan.FromSeconds(int64(1))

    let startInternal() =
        let mutable startTimeStamp = DateTimeOffset.MinValue

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

    /// Adds a new synchronous job to the scheduler.
    member this.NewJob cronExpr tzInfo job =
        match areJobsMonitored with
        | true -> failwith "Not implemented"
        | false -> jobs.Add(
            SyncJobDefinition(CronExpression.Parse(cronExpr), tzInfo, job)
        )

    /// Adds a new asynchronous job to the scheduler.
    member this.NewAsyncJob cronExpr job tzInfo =
        match areJobsMonitored with
        | true -> jobs.Add(MonitoredAsyncJobDefinition(
                CronExpression.Parse(cronExpr),
                tzInfo,
                job
            ))
        | false -> jobs.Add(AsyncJobDefinition(
                CronExpression.Parse(cronExpr),
                tzInfo,
                job
            ))

    /// Method for adding and enabling monitoring of jobs.
    member this.AddMonitoring(settings: StorageSettings): unit =
        Monitoring.Monitor.Initialize settings
        areJobsMonitored <- true

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
