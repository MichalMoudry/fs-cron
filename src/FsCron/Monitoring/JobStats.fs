namespace FsCron.Monitoring

open System

type JobStats = {
    JobName: string
    StartDate: DateTimeOffset
    EndDate: DateTimeOffset
}
