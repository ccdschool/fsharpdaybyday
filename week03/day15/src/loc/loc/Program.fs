[<EntryPoint>]
let main argv = 
    let build_results (n, lines) =
        (n, LOC.count lines)

    argv |> Cli.get_locations 
         |> Filesystem.find_source_files 
         |> Filesystem.compile_source_lines
         |> build_results
         |> Console.report_result

    0