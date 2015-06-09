open System.Text.RegularExpressions

type Glyph = { Symbol:string; Value:int }

let glyphs = [{Symbol="M"; Value=1000}; {Symbol="CM"; Value=900};
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
        let singleCharGlyphSymbols = glyphs |> List.filter (fun g -> g.Symbol.Length = 1) 
                                            |> List.map (fun g -> g.Symbol)
                                            |> List.toArray
        let pattern = sprintf "^[%s]*$" (System.String.Join ("", singleCharGlyphSymbols))
        Regex.Match(n, pattern).Success


    let convert_from_roman roman =
        let map_digits_to_values (roman:string) =
            let digit2value (d:char) =
                let d' = d.ToString()
                let glyph = List.find (fun g -> g.Symbol = d') glyphs
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


    let convert_to_roman arabic =
        let factorize arabic =
            let rec factorize' arabic (glyphs: Glyph list) factors =
                if arabic = 0 then
                    factors
                else
                    let g = glyphs.Head
                    if arabic > g.Value then
                        factorize' (arabic - g.Value) glyphs (factors @ [g.Symbol])
                    else if arabic = g.Value then
                        factorize' (arabic - g.Value) glyphs.Tail (factors @ [g.Symbol])
                    else
                        factorize' arabic glyphs.Tail factors
                  
            factorize' arabic glyphs []

        let join (digits:string list) =
            System.String.Join ("", List.toArray digits)

        arabic |> factorize 
               |> join


    if is_roman_number number_to_convert then
      number_to_convert |> convert_from_roman
                        |> printfn "%d"
    else
      let (_, arabic) = System.Int32.TryParse(number_to_convert)
      arabic |> convert_to_roman 
             |> printfn "%s"

    0