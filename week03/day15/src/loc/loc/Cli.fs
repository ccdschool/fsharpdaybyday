module Cli

let get_locations (argv:string array) =
    match argv with
    | [||] -> printfn "Usage: loc fileorfolder { fileorfolder }"
              System.Environment.Exit(1)
              [] // empty list only for compiler; runtime will never arrive here due to Exit()
    | locations -> locations |> Array.toList