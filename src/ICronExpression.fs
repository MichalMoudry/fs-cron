namespace FsCron

open System

/// A construct that wraps a cron expression and related operations.
type ICronExpression =
    abstract member Parse:
        expression: string * includesSeconds: bool -> ICronExpression
    abstract member TryParse:
        expression: string * includesSeconds: bool -> outref<ICronExpression>
    abstract member GetNextOccurrence:
        from: DateTimeOffset * timeZone: TimeZoneInfo * ?inclusive: bool -> DateTimeOffset
