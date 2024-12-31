namespace FsCron

open System
open System.Threading
open System.Threading.Tasks

[<Sealed>]
type internal MonitoredAsyncJobDefinition(
    cronExp,
    tzInfo,
    job: Func<CancellationToken, Task>) =
    inherit JobDefinition(cronExp, tzInfo)
    member this.ExecuteAsync(token: CancellationToken) =
        task {
            let start = DateTimeOffset.Now
            do! job.Invoke(token)
            this.UpdateNextOccurrence()
            Monitoring.Monitor.StoreJobStatistics
                {
                    JobName = $"{job.ToString()}_{start}"
                    StartDate = start
                    EndDate = DateTimeOffset.Now
                }
                |> ignore
        }
