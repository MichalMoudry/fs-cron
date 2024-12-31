namespace FsCron.Monitoring

/// An enum with possible places to store or display job statistics.
type OutputType =
    /// Job statistics will be printed in console.
    | Console = 0
    /// Job statistics will be stored in a remote cache.
    | RemoteCache = 1
