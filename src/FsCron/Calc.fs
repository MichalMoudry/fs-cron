/// A module providing methods for various calculations.
[<Sealed>]
module internal FsCron.Calc

open System

let inline GetNextJobOccurrenceDiff (nextOccurence: DateTimeOffset) =
    Math.Round((nextOccurence - DateTimeOffset.Now).TotalSeconds)
