// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main (argv:string array) = 
    let get_param = 
        if argv.Length > 0 then
            argv.[0]
        else
            "default"

    printfn "%s" get_param

    let get_param' =
        if argv.Length > 0 then
            (true, argv.[0])
        else
            (false, "")
    
    let default_param (param_found, param_value) =
        if param_found then
            param_value
        else
            "default"
    
    get_param' |> default_param |> printfn "%A"


    let get_param'' =
        if argv.Length > 0 then
            Some argv.[0]
        else
            None

    let default_param' = function
        | Some p -> p
        | None -> "default"

    let process_value p =
        printfn "<%s>" p

    get_param'' |> default_param' |> printfn "%s"


    let tryparse s = match System.Int32.TryParse(s) with
                        | (true, n) -> Some n
                        | _ -> None

    tryparse "42" |> printfn "%A" 


    let is_roman_number n =
        let (wasArabic, _) = System.Int32.TryParse(n)
        not wasArabic

    is_roman_number "XLII" |> printfn "%b"
    is_roman_number "42" |> printfn "%b"


    let convert_to_roman a = "XLII"
    let convert_to_arabic r = "42"


    let tryparse_int t =
        match System.Int32.TryParse(t) with
        | (true, i) -> Some i
        | (false, _) -> None

    let choose_action_on_type n onArabic onRoman =
        match tryparse_int n with
        | Some i -> onArabic i
        | None -> onRoman n


    let n = "42"
    match tryparse_int n with
        | Some i -> i |> convert_to_roman |> printfn "%s"
        | None -> n |> convert_to_arabic |> printfn "%s"

    choose_action_on_type n
        (fun a -> a |> convert_to_roman |> printfn "%s")
        (fun r -> r |> convert_to_arabic |> printfn "%s")

    0 // return an integer exit code

