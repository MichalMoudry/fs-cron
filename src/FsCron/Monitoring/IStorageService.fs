namespace FsCron.Monitoring

/// A generic storage service.
type internal IStorageService =
    abstract member StoreKeyVal<'a>: key: string -> value: 'a -> bool
