module Cli

exception WrongUsage

let get_locations (argv:string array) : Option<Datamodel.Locations> =
    match argv with
    | [||] -> None
    | locations -> locations |> Array.toList |> Some