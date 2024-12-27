namespace FsCron

open System
open System.Collections.Generic
open System.Threading
open Cronos

/// A job scheduler that runs in either synchronous or asynchronous mode.
[<Sealed>]
type Scheduler(cancellationToken: CancellationToken) =
    let jobs = List<JobDefinition>()

    let startInternal() =
        let mutable startTimeStamp = DateTimeOffset.MinValue
        let maxIterationDuration = TimeSpan.FromSeconds(int64(1))

        while not(cancellationToken.IsCancellationRequested) do
            startTimeStamp <- DateTimeOffset.Now
            for job in jobs do
                //(job.NextOccurrence - startTimeStamp).TotalSeconds
            Thread.Sleep(maxIterationDuration - (DateTimeOffset.Now - startTimeStamp))

    /// Adds a new asynchronous job to the scheduler.
    member this.NewAsyncJob cronExpr job tzInfo =
        jobs.Add(AsyncJobDefinition(CronExpression.Parse(cronExpr), tzInfo, job))

    /// Starts scheduler and blocks the current thread.
    member this.Start() = startInternal()

    /// Starts scheduler in an asynchronous manner in a
    /// separate background <seealso cref="Thread"/>.
    member this.StartAsync() = Thread(startInternal, IsBackground = true).Start()
