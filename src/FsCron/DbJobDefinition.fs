namespace FsCron

open System
open System.Data
open System.Threading
open System.Threading.Tasks

[<Sealed>]
type internal DbJobDefinition(
    cronExpr,
    tzInfo,
    job: Func<IDbConnection, CancellationToken, Task>) =
    inherit JobDefinition(cronExpr, tzInfo)
    member this.ExecuteAsync(dbConn: IDbConnection, token: CancellationToken) =
        task {
            do! job.Invoke(dbConn, token)
            this.UpdateNextOccurrence()
        }
