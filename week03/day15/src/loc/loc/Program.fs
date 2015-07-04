// What's new?
//  - System.Environment.Exit()
//  - modules
//  - code spread across files
//  - list comprehension with yield
//  - ref variables

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