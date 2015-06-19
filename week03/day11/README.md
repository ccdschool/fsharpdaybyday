# Day #11
Today's the day to implement the final feature of the "Convert roman" kata: converting arabic numbers to roman numbers.

```
let convert_to_roman arabic = // ...
```

## Solution design
How can this conversion be done - conceptually?

There is one thing for sure: Roman numbers consist of symbols representing certain values, e.g. I, V, C standing for 1, 5, 100. Which of these symbols to use depends on which values "fit into" a given arabic number. For 17 this would be 10, 5, 1 plus another 1. We could say these specific values are the factors to make up 17.

Once found, the symbols of the factors just need to be concatenated into a roman number string: X + V + I + I = XVII.

But what's with those pesky smaller symbols causing value reduction, e.g. the I in IV or the X in XC? What are the factors of for example 14? 10, 1, 1, 1, 1 leading to XIIII would technically work - but is not universally accepted as a valid roman number. XIV would be the mostly expected result.

Fortunately there are only a few such possible combinations, which could be called _syllables_: IV, IX, XL, XC, CD, CM. Since conversion starts from arabic numbers, i.e. with values, they can be treated like single roman digits. The factors of 14 then would be 10 (digit X) and 4 (syllable IV), those of 42 would be 40 (XL), 1 (I), 1 (I).

The solution steps now seem to be clear:

1. Factorize the arabic number, e.g. 42 becomes [40, 1, 1]
2. Translate the factors into symbols, e.g. [40, 1, 1] becomes ["XL", "I", "I"]
3. Finally build the roman number from the symbols (digits and syllables) , e.g. ["XL", "I", "I"] is combined into "XLII"

## Implementation
Implementing the steps is straightforward with what you already know. The factorization can be done recursively. See how the result is accumulated stepwise in the last parameter - and the first parameter is a stepwise "shrinking problem":

```
let factorize arabic =
    let values = [1000; 900; 500; 400; 100; 90; 
    				 50; 40; 10; 9; 5; 4; 1]

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

    factorize' arabic values []
```

Translating the values then into the roman symbols is easy, too. F# provides a read-only dictionary type to be filled with tuples as key-value-pairs:

```
let symbolize factors =
    let symbols =  dict [(1000, "M"); (900, "CM"); (500, "D"); (400, "CD");
    					   (100, "C"); (90, "XC"); (50, "L"); (40, "XL"); 
    					   (10, "X"); (9, "IX"); (5, "V"); (4, "IV"); (1, "I")]

    factors |> List.map (fun k -> symbols.Item(k))
```

Which leads to the full solution of the arabic to roman conversion:

```
let convert_to_roman arabic =
    let symbols = arabic |> factorize |> symbolize |> List.toArray
    System.String.Join("", symbols)
```

Problem solved. Case closed.

Or maybe not. Because the solution does not really feel DRY. There are two occurrences of the values; they are used during factorization as well as during symbolization.

This can be improved by extracting the _symbols_ dictionary from _symbolize_ and share it with _factorize_. Factorization can then get the values needed from the dictionary:

```
let values = symbols.Keys |> Seq.toList
factorize' arabic values []
```




type alias (for conscious constraints)



record type



day 12: module und namespaces - code aufteilen

day 13: match - für entscheidungen wie bei digit2value oder is_roman_number (mit option type)