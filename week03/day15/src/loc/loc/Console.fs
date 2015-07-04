module Console

let report_result (numberOfFiles, totalLinesOfCode) =
    printfn "%d source files found with %d lines of code" numberOfFiles totalLinesOfCode

let report_error errormsg =
    System.Console.ForegroundColor <- System.ConsoleColor.Red
    printfn "%s" errormsg

let report_exception ex =
    System.Console.BackgroundColor <- System.ConsoleColor.Red
    printfn "An unexpected situation arose: %A" ex