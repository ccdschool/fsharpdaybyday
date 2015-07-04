// What's new?
//  - multiple expressions in same line with ";"
//  - modules
//  - code spread across files
//  - list comprehension with yield
//  - ref variables

[<EntryPoint>]
let main argv = 
    let build_result (n, lines) =
        (n, LOC.count lines)

    try
        argv |> Cli.get_locations 
             |> Filesystem.find_source_files 
             |> Filesystem.compile_source_lines
             |> build_result
             |> Console.report_result

        0
    with
    | Cli.WrongUsage -> Console.report_error "Usage: loc fileOrFolder {fileOrFolder}"; 1
    | x -> Console.report_exception x; 99