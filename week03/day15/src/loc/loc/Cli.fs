module Cli

exception WrongUsage

let get_locations (argv:string array) =
    match argv with
    | [||] -> raise WrongUsage
    | locations -> locations |> Array.toList