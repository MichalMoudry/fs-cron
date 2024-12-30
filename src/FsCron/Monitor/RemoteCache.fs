/// Module containing code related to a remote cache storage (e.g. Redis or Garnet).
[<Sealed>]
module FsCron.Monitor.RemoteCache

open System.Text.Json
open StackExchange.Redis

let mutable private redis = Option<ConnectionMultiplexer>.None

let Connect (connStr: string) =
    redis <- Some(ConnectionMultiplexer.Connect(connStr))

let Store<'a> (key: string) (value: 'a) =
    if redis.IsSome then
        redis.Value
            .GetDatabase()
            .StringSet(key, JsonSerializer.Serialize(value))
    else
        false
