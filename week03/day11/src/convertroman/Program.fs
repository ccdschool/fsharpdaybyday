open System.Text.RegularExpressions

type ArabicNumber = int
type RomanNumber = string

type Symbol = { Text:string; Value:int }

let symbols = [{Text="M"; Value=1000}; {Text="CM"; Value=900};
               {Text="D"; Value=500}; {Text="CD"; Value=400};
               {Text="C"; Value=100}; {Text="XC"; Value=90};
               {Text="L"; Value=50}; {Text="XL"; Value=40};
               {Text="X"; Value=10}; {Text="IX"; Value=9};
               {Text="V"; Value=5}; {Text="IV"; Value=4};
               {Text="I"; Value=1}]


[<EntryPoint>]
let main argv = 
    let number_to_convert = argv.[0]

    let is_roman_number n =
        let singleCharSymbols = symbols |> List.filter (fun s -> s.Text.Length = 1) 
                                        |> List.map (fun s -> s.Text)
                                        |> List.toArray
        let pattern = sprintf "^[%s]*$" (System.String.Join ("", singleCharSymbols))
        Regex.Match(n, pattern).Success


    let convert_from_roman (roman:RomanNumber) : ArabicNumber =
        let map_digits_to_values (roman:RomanNumber) =
            let valueOf (digit:char) =
                let symbol = symbols |> List.find (fun s -> s.Text = digit.ToString())
                symbol.Value
            roman.ToCharArray() |> Array.map valueOf

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
            let rec symbolize' arabic (symbols: Symbol list) factors =
                if arabic = 0 then
                    factors
                else
                    let s = symbols.Head
                    if arabic > s.Value then
                        symbolize' (arabic - s.Value) symbols (factors @ [s.Text])
                    else if arabic = s.Value then
                        symbolize' (arabic - s.Value) symbols.Tail (factors @ [s.Text])
                    else
                        symbolize' arabic symbols.Tail factors
                  
            symbolize' arabic symbols []

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