namespace FsCron

open System.Collections.Generic
open System.Threading

[<Sealed>]
type Scheduler(cancellationToken: CancellationToken) =
    let jobs = List<JobDefinition>()
    let startInternal() =
        ()

    /// Starts scheduler and blocks the current thread.
    let Start() = startInternal()

    /// Starts scheduler in an asynchronous manner in a
    /// separate background <seealso cref="Thread"/>.
    let StartAsync() = Thread(startInternal, IsBackground = true).Start()
