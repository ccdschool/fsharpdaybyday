// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

module Contracts =
    type CountingResult = {NumberOfFiles:int; TotalLinesOfCode:int}

module Cli =
    let get_locations (argv:string array) =
        ["a.cs"; "./sampledata"]

module Console =
    let report_result (results:Contracts.CountingResult) =
        printfn "%d source files found with %d lines of code" results.NumberOfFiles results.TotalLinesOfCode

module Filesystem =
    let find_source_files (locations:string list) =
        seq ["a.cs"; "./sampledata/b.cs"; "./sampledata/c.cs"]

    let compile_source_lines (filenames:string seq) =
        (3, seq ["1"; "//"; "2 //"; "  //"])

module LOC =
    let count (lines:string seq) =
        42


[<EntryPoint>]
let main argv = 
    let build_results (n, lines) : Contracts.CountingResult =
        {NumberOfFiles=n; 
         TotalLinesOfCode=LOC.count lines}

    argv |> Cli.get_locations 
         |> Filesystem.find_source_files 
         |> Filesystem.compile_source_lines
         |> build_results
         |> Console.report_result

    0