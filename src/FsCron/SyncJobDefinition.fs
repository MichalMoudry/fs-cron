namespace FsCron

open System

[<Sealed>]
type SyncJobDefinition(cronExp, tzInfo, job: Action) =
    inherit JobDefinition(cronExp, tzInfo)
    member this.Execute() =
        job.Invoke()
        this.UpdateNextOccurrence()
