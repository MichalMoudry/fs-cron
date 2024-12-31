namespace FsCron.Monitoring.Storage

open System.Text.Json
open StackExchange.Redis

/// An object with singleton lifecycle, because is models.
[<Sealed>]
type internal RemoteCache private () =
    let mutable redis = Option<ConnectionMultiplexer>.None

    static let mutable instance = Option<RemoteCache>.None
    static member Instance with get() =
        if instance.IsNone then
            instance <- Some(RemoteCache())
        instance.Value

    /// Method for connecting to a remote cache
    /// and creating a singleton connection object.
    member this.Connect (connStr: string) =
        redis <- Some(ConnectionMultiplexer.Connect(connStr))

    interface IStorageService with
        member this.StoreKeyVal key value =
            if redis.IsSome then
                redis.Value
                    .GetDatabase()
                    .StringSet(key, JsonSerializer.Serialize(value))
            else
                false
