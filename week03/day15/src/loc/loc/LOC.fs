namespace Domain

module LOC =
    let internal count (lines:string list) =
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
                        let iOpenComment = l'.IndexOf("/*")
                        let iCloseComment = l'.IndexOf("*/")

                        inComment := iOpenComment >= 0 && iCloseComment < 0

                        if iOpenComment <> 0 then 
                            yield l
                        else 
                            if iCloseComment > 0 && iCloseComment < (l.Length-2) then yield l
            ]

        let single_line_comments (line:string) =
            not (line.Trim().StartsWith("//"))

        lines |> List.filter whitespace
              |> filter_multi_line_comments
              |> List.filter single_line_comments 
              |> List.length

    (* Limitations of solution:
        - does not account for a single line comment containing a multi line comment
        - does not account for a multi line comment opening in the same line as a previous one closes
        - does not detect nested multi line comments
    *)


    let analyze_source_lines (n, lines) : Datamodel.Result =
        {Datamodel.numberOfFiles=n; 
         Datamodel.totalLinesOfCode=count lines}