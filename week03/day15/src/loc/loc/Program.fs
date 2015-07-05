// What's new?
//  - multiple expressions in same line with ";"
//  - modules
//  - code spread across files
//  - list comprehension with yield
//  - ref variables
//  - autom. tests
//  - function composition

// Applied:
//  - exceptions
//  - Option type

[<EntryPoint>]
let main argv = 
    let get_locations =
        argv |> Cli.get_locations

    let gather_source_lines =
        Filesystem.find_source_files >> Filesystem.compile_source_lines

    let count_lines (n, lines) =
        (n, LOC.count lines)

    try
        match get_locations with
                | Some locations ->
                          locations |> gather_source_lines
                                    |> count_lines
                                    |> Console.report_result
                          0
                | None -> Console.report_error "Usage: loc fileOrFolder {fileOrFolder}"
                          1
    with
    | x -> Console.report_exception x; 99