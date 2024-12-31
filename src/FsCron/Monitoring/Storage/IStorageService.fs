namespace FsCron.Monitoring.Storage

/// A generic storage service.
type internal IStorageService =
    /// Method for storing a key/value.
    abstract member StoreKeyVal<'a>: key: string -> value: 'a -> bool
