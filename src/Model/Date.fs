namespace FsCron.Model

/// An internal Date structure similar to <see cref="System.Date"/> but with mutable members.
[<Struct>]
type internal Date =
    val mutable Second: int
    val mutable Minute: int
    val mutable Hour: int
    val mutable Day: int
    val mutable Month: int
    val mutable Year: int
