[<Sealed>]
module internal FsCron.CronHelper

open System
open FsCron.Model

(*let private parseMinutes(def: string) =
    if def.Length > 4 then
        int16(0)
    else
        match def[0..1] with
        | "*/" -> Convert.ToInt16(def[1..])
        | _ -> Convert.ToInt16(def)

let private parse(parts: string[]) =
    for i = 0 to parts.Length do
        printfn $"Iter: {i}"
        match i with
        | 0 -> parseMinutes(parts[i])
        | _ -> int16(0)
        |> ignore
    ()*)

/// A method for parsing cron schedule definition.
let ParseCron(str: string) =
    let parts = str.Split()
    if parts.Length <> 5 then
        failwith "Invalid length"

    let dayOfWeek =
        match parts[4] with
        | "0" -> WeekDay.Sunday
        | "1" -> WeekDay.Monday
        | "2" -> WeekDay.Tuesday
        | "3" -> WeekDay.Wednesday
        | "4" -> WeekDay.Thursday
        | "5" -> WeekDay.Friday
        | "6" -> WeekDay.Saturday
        | "*" -> WeekDay.All
        | _ -> failwith "invalid day of week"

    let month =
        match parts[3] with
        | "*" -> Month.All
        | _ -> Enum.Parse<Month>(parts[3])
    (*let isValid, month = Int32.TryParse(parts[3])
    if not(isValid) || not(month > 0 && month < 13) then
        failwith "invalid month"*)
    ()
