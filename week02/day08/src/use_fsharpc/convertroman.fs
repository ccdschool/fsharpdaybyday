[<EntryPoint>]
let main argv = 
    let number_to_convert = argv.[0]
    let is_roman_number n = true
    let convert_from_roman roman = 42
    let convert_to_roman arabic = "XLII"

    if is_roman_number number_to_convert then
      let arabic_number = convert_from_roman number_to_convert
      printfn "%d" arabic_number
    else
      let roman_number = convert_to_roman number_to_convert
      printfn "%s" roman_number
    0