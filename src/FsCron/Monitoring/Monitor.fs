[<Sealed>]
module internal FsCron.Monitoring.Monitor

open System.Text.Json
open FsCron.Monitoring.Storage

let mutable private storageService = Option<IStorageService>.None

/// Initialization method for the Monitor module.
let Initialize storageSettings =
    if storageSettings.Type = OutputType.RemoteCache then
        RemoteCache.Instance.Connect(storageSettings.ConnectionString)
        storageService <- Some(RemoteCache.Instance)
    elif storageSettings.Type = OutputType.Console then
        storageService <- Some(ConsoleOutput())

let StoreJobStatistics (stats: JobStats) =
    match storageService with
    | None -> false
    | Some service -> service.StoreKeyVal stats.JobName (JsonSerializer.Serialize(stats))
