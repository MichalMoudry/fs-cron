namespace FsCron

open System.Threading

[<Sealed>]
type Scheduler() =
    let startInternal() =
        while true do
            printfn "TODO: Run job check"
            Thread.Sleep(1000)
    member this.Start() =
        let thread = Thread(ThreadStart(startInternal))
        thread.IsBackground <- true
        thread.Start()
    member this.Stop() = ()
