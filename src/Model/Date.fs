namespace FsCron.Model

open System

/// An internal Date structure similar to <see cref="System.Date"/> but with mutable members.
type internal Date =
    struct
        val mutable Second: int
        val mutable Minute: int
        val mutable Hour: int
        val mutable Day: int
        val mutable Month: int
        val mutable Year: int
        new(year, month, day) = { Second = 0; Minute = 0; Hour = 0; Day = day; Month = month; Year = year }
    end
    member this.ToDateTime() =
        DateTime(this.Year, this.Month, this.Day, this.Hour, this.Minute, this.Second)
