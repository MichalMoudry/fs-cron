namespace FsCron.Monitoring.Storage

open System

[<Sealed>]
type internal ConsoleOutput() =
    interface IStorageService with
        member this.StoreKeyVal key value =
            Console.WriteLine($"[{key}] {value}")
            true
