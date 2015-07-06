namespace Adapters

module Cli =
    open Datamodel

    let get_locations (argv:string array) : Option<Locations> =
        match argv with
        | [||] -> None
        | locations -> locations |> Array.toList |> Some