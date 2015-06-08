open System.Text.RegularExpressions

[<EntryPoint>]
let main argv = 
    let number_to_convert = argv.[0]

    let is_roman_number n =
      Regex.Match(n, "^[IVXLCDM]*$").Success


    let convert_from_roman roman =
        let map_digits_to_values (roman:string) = // (roman:string)
            let digit2value d =
                if      d = 'I' then 1
                else if d = 'V' then 5
                else if d = 'X' then 10
                else if d = 'L' then 50
                else if d = 'C' then 100
                else if d = 'D' then 500
                else                 1000
            roman.ToCharArray() |> Array.map digit2value // |>

        let negate_smaller_values values =
            let nvalues = Array.copy values
            List.iter (fun i -> nvalues.[i] <- nvalues.[i] * (if nvalues.[i] < nvalues.[i+1] then -1 else 1)) 
                      [0..nvalues.Length-2]
            nvalues
        
        roman |> map_digits_to_values
              |> negate_smaller_values
              |> Array.sum


    let convert_to_roman arabic = "XLII"


    if is_roman_number number_to_convert then
      number_to_convert |> convert_from_roman
                        |> printfn "%d"
    else
      number_to_convert |> convert_to_roman 
                        |> printfn "%s"

    0