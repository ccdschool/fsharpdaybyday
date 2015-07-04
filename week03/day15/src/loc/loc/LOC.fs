module LOC

let count (lines:string list) =
    let filter_multi_line_comments (lines:string list) =
        [
            let inComment = ref false
            for l in lines do
                let l' = l.Trim()

                if !inComment then
                    inComment := not (l'.EndsWith("*/"))
                else
                    inComment := l'.StartsWith("/*")
                    if not !inComment then yield l
                    inComment := !inComment && not (l'.EndsWith("*/")) // account for multi line comment in one line
        ]

    let single_line_comments (line:string) =
        not (line.Trim().StartsWith("//"))

    lines |> filter_multi_line_comments
          |> List.filter single_line_comments |> List.length