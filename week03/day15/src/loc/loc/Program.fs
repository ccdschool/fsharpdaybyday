// What's new?
//  - multiple expressions in same line with ";"
//  - modules
//  - code spread across files
//  - list comprehension with yield
//  - ref variables
//  - unit tests
//  - function composition

// Applied:
//  - exceptions
//  - Option type
//  - data flow
//  - data model

[<EntryPoint>]
let main argv = 
    let get_locations : Option<Datamodel.Locations> =
        argv |> Cli.get_locations

    let acquire_source_lines =
        Filesystem.find_source_files >> Filesystem.compile_source_lines

    let analyze_source_lines (n, lines) : Datamodel.Result =
        {Datamodel.numberOfFiles=n; 
         Datamodel.totalLinesOfCode=LOC.count lines}

    try
        match get_locations with
                | Some locations ->
                          locations |> acquire_source_lines
                                    |> analyze_source_lines
                                    |> Console.report_result
                          0
                | None -> Console.report_error "Usage: loc fileOrFolder {fileOrFolder}"
                          1
    with
    | x -> Console.report_exception x; 99