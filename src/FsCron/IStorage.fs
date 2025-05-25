namespace FsCron

type IStorage =
    abstract member Add<'T>: string -> 'T -> unit
    abstract member Get<'T>: string -> 'T
