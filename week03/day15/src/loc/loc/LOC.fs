module LOC

let count (lines:string list) =
    let whitespace (line:string) =
        line.Trim() <> ""

    let filter_multi_line_comments (lines:string list) =
        [
            let inComment = ref false
            for l in lines do
                let l' = l.Trim()

                if !inComment then
                    inComment := l'.IndexOf("*/") < 0
                    if not !inComment && not (l'.EndsWith("*/")) then yield l
                else
                    inComment := l'.IndexOf("/*") >= 0
                    if not (l'.StartsWith("/*")) then yield l
                    inComment := !inComment && not (l'.EndsWith("*/")) // account for multi line comment in one line
        ]

    let single_line_comments (line:string) =
        not (line.Trim().StartsWith("//"))

    lines |> List.filter whitespace
          |> filter_multi_line_comments
          |> List.filter single_line_comments |> List.length

(* Limitations of solution:
    - does not account for a single line comment containing a multi line comment
    - does not account for a multi line comment opening in the same line as a previous one closes
    - does not detect nested multi line comments
*)