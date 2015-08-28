open Adapters
open Domain

[<EntryPoint>]
let main argv =      
    try
        argv |> Cli.get_locations 
             |> function
                | Some locations ->
                          locations |> Filesystem.acquire_source_lines
                                    |> LOC.analyze_source_lines
                                    |> Console.report_result
                          0
                | None -> Console.report_error "Usage: loc fileOrFolder {fileOrFolder}"
                          1
    with
    | x -> Console.report_exception x; 99