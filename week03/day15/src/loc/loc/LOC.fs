module LOC

let count (lines:string list) =
    let single_line_comments (line:string) =
        not (line.Trim().StartsWith("//"))

    lines |> List.filter single_line_comments |> List.length