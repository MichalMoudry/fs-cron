namespace FsCron.Storage.Redis

open System.Text.Json
open FsCron
open StackExchange.Redis

[<Sealed>]
type internal RemoteCacheStorage(connStr: string) =
    let cache = ConnectionMultiplexer.Connect(connStr)
    interface IStorage with
        member this.Add key value =
            cache
                .GetDatabase()
                .ListRightPushAsync(key, JsonSerializer.Serialize(value))
        member this.Get key index =
            task {
                let! value =
                    cache.GetDatabase().ListGetByIndexAsync(key, index)
                return JsonSerializer.Deserialize<'a>(value)
            }
