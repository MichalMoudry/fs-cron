namespace FsCron

open System
open System.Threading
open System.Threading.Tasks

[<Sealed>]
type internal AsyncJobDefinition(cronExp, tzInfo, job: Func<CancellationToken, Task>) =
    inherit JobDefinition(cronExp, tzInfo)
    member this.ExecuteAsync(token: CancellationToken) =
        task {
            do! job.Invoke(token)
            this.UpdateNextOccurrence()
        }
