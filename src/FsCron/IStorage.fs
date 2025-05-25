namespace FsCron

open System.Threading.Tasks

type IStorage =
    abstract member Add<'T>: string -> 'T -> Task
    abstract member Get<'T>: string -> int64 -> Task<'T>
