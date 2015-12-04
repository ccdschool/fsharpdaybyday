[← Day #10 – Operator definition and type constraints](../../week02/day10) | [Day #12 – Pattern matching →](../day12)

# Day #11 - Type aliases and record types
Today's the day to implement the final feature of the "Convert roman" kata: converting arabic numbers to roman numbers.

```fsharp
let convert_to_roman arabic = // ...
```

## Solution design
How can this conversion be done - conceptually?

There is one thing for sure: Roman numbers consist of symbols representing certain values, e.g. I, V, C standing for 1, 5, 100. Which of these symbols to use depends on which values "fit into" a given arabic number. For 17 this would be 10, 5, 1 plus another 1. We could say these specific values are the factors to make up 17.

Once found, the symbols of the factors just need to be concatenated into a roman number string: X + V + I + I = XVII.

But what's with those pesky smaller symbols causing value reduction, e.g. the I in IV or the X in XC? What are the factors of for example 14? 10, 1, 1, 1, 1 leading to XIIII would technically work - but is not universally accepted as a valid roman number. XIV would be the mostly expected result.

Fortunately there are only a few such possible combinations, which could be called _syllables_: IV, IX, XL, XC, CD, CM. Since conversion starts from arabic numbers, i.e. with values, they can be treated like single roman digits. The factors of 14 then would be 10 (digit X) and 4 (syllable IV), those of 42 would be 40 (XL), 1 (I), 1 (I).

The solution steps now seem to be clear:

1. Factorize the arabic number, e.g. `42` becomes `[40, 1, 1]`
2. Translate the factors into symbols, e.g. `[40, 1, 1]` becomes `["XL", "I", "I"]`
3. Finally build the roman number from the symbols (digits and syllables) , e.g. `["XL", "I", "I"]` is combined into `"XLII"`

## Implementation
Implementing the steps is straightforward with what you already know. The factorization can be done recursively. See how the result is accumulated stepwise in the last parameter - and the first parameter is a stepwise "shrinking problem":

```fsharp
let factorize arabic =
    let values = [1000; 900; 500; 400; 100; 90; 
    				 50; 40; 10; 9; 5; 4; 1]

    let rec factorize' arabic (values: int list) factors =
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

```fsharp
let symbolize factors =
    let symbols =  dict [(1000, "M"); (900, "CM"); (500, "D"); (400, "CD");
    					   (100, "C"); (90, "XC"); (50, "L"); (40, "XL"); 
    					   (10, "X"); (9, "IX"); (5, "V"); (4, "IV"); (1, "I")]

    factors |> List.map (fun k -> symbols.[k])
```

Which leads to the full solution of the arabic to roman conversion:

```fsharp
let convert_to_roman arabic =
    let symbols = arabic |> factorize |> symbolize |> List.toArray
    System.String.Join("", symbols)
```

Problem solved. Case closed.

Or maybe not. Because the solution does not really feel DRY. There are two occurrences of the values; they are used during factorization as well as during symbolization.

This can be improved by extracting the `symbols` dictionary from `symbolize` and share it with `factorize`. Factorization can then get the values needed from the dictionary:

```fsharp
let values = symbols.Keys |> Seq.toList
factorize' arabic values []
```

Problem solved. Case closed.

Or maybe not. Because the overall solution of converting to/from roman numbers still is not DRY. Both conversion directions need the symbols - but in different ways.

Converting to roman numbers goes from value to symbol text. Converting from roman numbers, though, goes from symbol text to value (and is only interested in digit symbols, not syllables).

Using a dictionary does not seem the best fit to support both conversions equally. A dictionary's key-value-pairs are asymmetric. But what's needed is a symmetric represenation of symbols.

A list of tuples instead of a dictionary would do - but makes it hard to access the tuple parts. Remember: tuples need to be decomposed to get at their members:

```fsharp
let person = ("Peter", 42)
let (name, age) = person
```

## Type alias
Tuples are ad hoc data types. No declaration needed, just throw together a couple of values and let F# bind them together into a new type. For the above `person` tuple value that would be `string * int` as explained on day #9.

But what about "real" types? Of course F# lets you define your own types. Here is the simplest way to do this:

```fsharp
type Distance = float
type Factor = int
type Name = string
type Tags = string list
type Names = Name list
```

These types are called _aliases_. They just give alternative names to other types. See them as shortcuts. And as a way to add meaning. Compare these function definitions:

```fsharp
let print friends =
    ...
    
let print' (friends: string list) =
    ...
    
let print'' (friends: Names) =
    ...
```

They are progressively more specific and more meaningful. To explicitly make `friends` to be of type `Names` compared to `string list` makes the function definition more expressive. Also it makes it easier to change the type of `friends` wherever this kind of data is used, e.g.

```fsharp
let print (friends: Names) =
	...
let aggregate (friends: Names) =
	...
let store (friends: Names) =
	...
```

If you wanted to change the data structure holding `friends` from `string list` for example to `string array` you'd need to do that only in one place.

Type aliases are useful to give meaning - but they don't add real constrains. As you can see, assignment across type aliases is possible as long as the underlying type is the same:

```fsharp
type A = int
type B = int

let (a:A) = 5
let (b:B) = a
```

That's not what you usually want when you start using your own types.

## Tuple types
As you've seen you can give aliases not only to scalar types like `int` or `bool` but also to collection types like `string list`. The same is true for tuples.

If you want to constrain (or document) a function expecting a tuple you can define your own tuple type:

```fsharp
type Person = string * int

let print (employee: Person) =
    printfn "%A" employee
    
print ("Peter", 42)
```

This is equivalent to, but shorter and easier to change than

```fsharp
let print (name: string, age: int) =
	...
```

## Record type
Tuples are structures of a fixed number of heterogeneous elements - which are anonymous. Only when deconstructing a tuple meaning is assigned to its fields.

If that's too late for your purpose or too cumbersome or not meaningful enough, then you can define record types instead. They are like tuples but with named fields.

```fsharp
type Person = {Name:string; Age:int}
```

To create a record value assign values to its fields like this:

```fsharp
let p = {Name="Peter"; Age=42}
```

Each field must be assigned to! That's necessary since record types like all other F# types are immutable by default. You want to be sure all fields have values right after record creation. No "partial creation" allowed.

When you want to access record fields do it like in many other languages, use the `. operator:

```fsharp
printfn "%s is %d years old" p.Name p.Age
```

When you create a record value F# usually infers the type correctly from the field names used. But in case two record types look the same (or if you want to add more explicitness to your code) you can specify the type using a constraint, e.g.

```fsharp
let p:Person = {Name="Mary"; Age=38}
```

Records once created are immutable. But F# helps you to create new records from existing ones - and at the same time change a couple of fields, e.g.

```fsharp
let q = {p with Name="Bella"}
```

Instead of filling in all fields use an existing record value like `p` in the previous example and then - after `with` - assign values to fields that you want to change.

## Using records in roman conversion
With records under our belt we can rework our roman conversion approach.

First let's mirror the two main domain language terms in the code: arabic number and roman number.

```fsharp
type ArabicNumber = int
type RomanNumber = string

let convert_from_roman (roman:RomanNumber) : ArabicNumber =
	...
let convert_to_roman (arabic:ArabicNumber) : RomanNumber =
	...
```

Constraining input parameters as well as the function result makes the function signatures very specific. It's now perfectly clear what's allowed to go in - what will be produced.

Even though F# in many cases does not need type annotations it can be helpful to at least annotate public or API functions - they can serve as beautiful documentation.

Secondly let's set up the core dictionary of the application mapping textual symbols to values.

```fsharp
type Symbol = { Text:string; Value:int }

let symbols = [{Text="M"; Value=1000}; {Text="CM"; Value=900};
               {Text="D"; Value=500}; {Text="CD"; Value=400};
               {Text="C"; Value=100}; {Text="XC"; Value=90};
               {Text="L"; Value=50}; {Text="XL"; Value=40};
               {Text="X"; Value=10}; {Text="IX"; Value=9};
               {Text="V"; Value=5}; {Text="IV"; Value=4};
               {Text="I"; Value=1}]
```

Even though this is conceptually a dictionary or mapping it's not so formally. Rather than using a `dict` type a `list` of records is used. Remember the requirement of symmetric data? Neither symbol text nor symbol value should have priority.

From this dictionary the list of valid characters in a roman number can be derived for number category checking:

```fsharp
let is_roman_number n =
    let singleCharSymbols = symbols |> List.filter (fun s -> s.Text.Length = 1) 
                                    |> List.map (fun s -> s.Text)
                                    |> List.toArray
    let pattern = sprintf "^[%s]*$" (System.String.Join ("", singleCharSymbols))
    Regex.Match(n, pattern).Success
```

Only the single character symbols like I or X or D are compiled into the Regex pattern - which formerly was a constant.

Then translating a roman digit to its value uses the list of symbols:

```fsharp
let map_digits_to_values (roman:RomanNumber) =
    let valueOf (digit:char) =
        let symbol = symbols |> List.find (fun s -> s.Text = digit.ToString())
        symbol.Value
    roman.ToCharArray() |> Array.map valueOf
    
    ...
```

This is a bit more elaborate since no real dictionary is used - but then it's easy to access the symbol's fields using the . operator than working with tuples.

And finally translating an arabic number into roman symbols reverses the access pattern:

```fsharp
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
```

Finding the factors and mapping the factors to their symbol texts have been merged into a single function. When the factor has been found, its string representation is at hand, too. So why not use it right away? See for example:

```fsharp
if arabic = s.Value then
    symbolize' (arabic - s.Value) symbols.Tail (factors @ [s.Text])
...
```

As usual you can find the complete source code in the [repo](src/convertroman/Program.fs).

***

Being able to easily define your own types is one of the great strengths of F#. It helps to manifest your domain language in code. More on that later...

### Read more
Type aliases

* Scott Wlaschin, [Type abbreviations - Also known as aliases](http://fsharpforfunandprofit.com/posts/type-abbreviations/)

Record type

* Microsoft, [Records (F#)](https://msdn.microsoft.com/en-us/library/dd233184.aspx)
* Scott Wlaschin, [Records - Extending tuples with labels](http://fsharpforfunandprofit.com/posts/records/)
* Wikibooks, [F Sharp Programming / Tuples and Records](https://en.wikibooks.org/wiki/F_Sharp_Programming/Tuples_and_Records)

[← Day #10 – Operator definition and type constraints](../../week02/day10) | [Day #12 – Pattern matching →](../day12)
