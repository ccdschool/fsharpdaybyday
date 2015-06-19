open System.Text.RegularExpressions

type ArabicNumber = int
type RomanNumber = string

type Syllable = { Symbol:string; Value:int }

let syllables = [{Symbol="M"; Value=1000}; {Symbol="CM"; Value=900};
                 {Symbol="D"; Value=500}; {Symbol="CD"; Value=400};
                 {Symbol="C"; Value=100}; {Symbol="XC"; Value=90};
                 {Symbol="L"; Value=50}; {Symbol="XL"; Value=40};
                 {Symbol="X"; Value=10}; {Symbol="IX"; Value=9};
                 {Symbol="V"; Value=5}; {Symbol="IV"; Value=4};
                 {Symbol="I"; Value=1}]


[<EntryPoint>]
let main argv = 
    let number_to_convert = argv.[0]

    let is_roman_number n =
        let singleCharSyllableSymbols = syllables |> List.filter (fun g -> g.Symbol.Length = 1) 
                                                  |> List.map (fun g -> g.Symbol)
                                                  |> List.toArray
        let pattern = sprintf "^[%s]*$" (System.String.Join ("", singleCharSyllableSymbols))
        Regex.Match(n, pattern).Success


    let convert_from_roman roman =
        let map_digits_to_values (roman:RomanNumber) : ArabicNumber array =
            let digit2value (d:char) =
                let d' = d.ToString()
                let glyph = List.find (fun g -> g.Symbol = d') syllables
                glyph.Value
            roman.ToCharArray() |> Array.map digit2value 

        let negate_smaller_values values =
            let nvalues = Array.copy values
            List.iter (fun i -> nvalues.[i] <- nvalues.[i] * (if nvalues.[i] < nvalues.[i+1] then -1 else 1)) 
                      [0..nvalues.Length-2]
            nvalues
        
        roman |> map_digits_to_values
              |> negate_smaller_values
              |> Array.sum


    let convert_to_roman (arabic:ArabicNumber) : RomanNumber =
        let symbolize arabic =
            let rec symbolize' arabic (syllables: Syllable list) factors =
                if arabic = 0 then
                    factors
                else
                    let s = syllables.Head
                    if arabic > s.Value then
                        symbolize' (arabic - s.Value) syllables (factors @ [s.Symbol])
                    else if arabic = s.Value then
                        symbolize' (arabic - s.Value) syllables.Tail (factors @ [s.Symbol])
                    else
                        symbolize' arabic syllables.Tail factors
                  
            symbolize' arabic syllables []

        let join (symbol:string list) =
            System.String.Join ("", List.toArray symbol)

        arabic |> symbolize 
               |> join


    if is_roman_number number_to_convert then
      number_to_convert |> convert_from_roman
                        |> printfn "%d"
    else
      let (_, arabic) = System.Int32.TryParse(number_to_convert)
      arabic |> convert_to_roman 
             |> printfn "%s"

    0