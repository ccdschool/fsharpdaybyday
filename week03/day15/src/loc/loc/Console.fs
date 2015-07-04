module Console

let report_result (results:Contracts.CountingResult) =
    printfn "%d source files found with %d lines of code" results.NumberOfFiles results.TotalLinesOfCode