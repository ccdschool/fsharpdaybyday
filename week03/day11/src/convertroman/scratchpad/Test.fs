namespace scratchpad

open System
open NUnit.Framework

[<TestFixture>]
type ToRomanConversion() =  
    let symbols =  dict [(1000, "M"); (900, "CM"); (500, "D"); (400, "CD"); 
                         (100, "C"); (90, "XC"); (50, "L"); (40, "XL"); 
                         (10, "X"); (9, "IX"); (5, "V"); (4, "IV"); (1, "I")]

    let factorize arabic =
        let rec factorize' arabic (values:int list) factors =
            if arabic = 0 then
                factors
            else 
                let f = values.Head
                if arabic > f then
                    factorize' (arabic - f) values (factors @ [f])
                else if arabic = f then
                    factorize' (arabic - f) values.Tail (factors @ [f])
                else
                    factorize' arabic values.Tail factors

        let values = symbols.Keys |> Seq.toList

        factorize' arabic values []

    let symbolize factors =
        factors |> List.map (fun k -> symbols.Item(k))

    let convert_to_roman arabic =
        let symbols = arabic |> factorize |> symbolize |> List.toArray
        System.String.Join("", symbols)


    [<Test>]
    member x.Symbolize() =
        Assert.AreEqual(["M"; "L"; "X"; "IX"; "I"], (symbolize [1000; 50; 10; 9; 1]))

    [<Test>]
    member x.Factorize() =
        Assert.AreEqual([40; 1; 1], (factorize 42))

    [<Test>]
    member x.convert() =
       Assert.AreEqual("XLII", (convert_to_roman 42))