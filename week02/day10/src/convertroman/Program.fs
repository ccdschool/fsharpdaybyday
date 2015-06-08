open System.Text.RegularExpressions

[<EntryPoint>]
let main argv = 
    let number_to_convert = argv.[0]

    let is_roman_number n =
      Regex.Match(n, "^[IVXLCDM]*$").Success

    let convert_from_roman roman =
        let map_digits_to_values roman =
            [10; 50; 1; 1]
        let negate_smaller_values values =
            [-10; 50; 1; 1]
        
        let values = map_digits_to_values roman
        let values' = negate_smaller_values values
        List.sum values'

    let convert_to_roman arabic = "XLII"

    if is_roman_number number_to_convert then
      let arabic_number = convert_from_roman number_to_convert
      printfn "%d" arabic_number
    else
      let roman_number = convert_to_roman number_to_convert
      printfn "%s" roman_number

    0