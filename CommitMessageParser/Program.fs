// Learn more about F# at http://fsharp.org

open System

[<EntryPoint>]
let main _ =
    async {
        let! data =  Query.getCommits ()
        let totals = Parser.getTotalCommits(data)
        printfn "%A" totals
    }
    |> Async.RunSynchronously
        
    printfn "Done!"
    Console.ReadKey() |> ignore
    0 
