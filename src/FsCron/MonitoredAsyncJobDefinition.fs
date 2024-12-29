namespace FsCron

type MonitoredAsyncJobDefinition(cronExp, tzInfo, job) =
    inherit AsyncJobDefinition(cronExp, tzInfo, job)
