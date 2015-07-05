module Cli

exception WrongUsage

let get_locations (argv:string array) =
    match argv with
    | [||] -> None
    | locations -> locations |> Array.toList |> Some