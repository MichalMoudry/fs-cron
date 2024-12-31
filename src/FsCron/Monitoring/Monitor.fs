[<Sealed>]
module internal FsCron.Monitoring.Monitor

open System.Text.Json

let mutable private storageService = Option<IStorageService>.None

/// Initialization method for the Monitor module.
let Initialize storageSettings =
    if storageSettings.Type = StorageType.RemoteCache then
        ()
    ()

let StoreJobStatistics (stats: JobStats) =
    match storageService with
    | None -> false
    | Some service -> service.StoreKeyVal stats.JobName (JsonSerializer.Serialize(stats))
