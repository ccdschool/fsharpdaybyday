module Cli

let get_locations (argv:string array) =
    match argv with
    | [||] -> printfn "Usage: loc location { location }"; System.Environment.Exit(1); [] // empty list will be ignored
    | locations -> locations |> Array.toList