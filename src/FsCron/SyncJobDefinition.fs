namespace FsCron

open System

type SyncJobDefinition(cronExp, tzInfo, job: Action) =
    inherit JobDefinition(cronExp, tzInfo)
    member this.Execute() =
        job.Invoke()
        this.UpdateNextOccurrence()
