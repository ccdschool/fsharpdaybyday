# Day #9 - Tuples
Today we'll focus on just one feature of the ["Convert roman" kata](https://app.box.com/s/z07b8gr6e1ngvb3cg7ps78zy2ddi3vx1): determine the type of number to be converted. Is it a roman number or an arabic one?

The function to accomplish this is

```
let is_roman_number n = ...
```

with a type of _string -> bool_.

But how to analyse the string? There are two ways to do that:

* Try to convert the string into an integer. If it succeeds, then it's an arabic number, otherwise assume it to be a roman number.
* Check the characters of the string to be in the set of valid roman digits (I, V, X, L, C, D, M).

Both can easily be done with functions from the .NET BCL: _TryParse()_ and _Regex.Match()_.

## Namespaces
Both function are static and can readily be used from F#:

```fsharp
let r = System.Int32.TryParse("42")
let m = System.Text.RegularExpressions.Regex.Match("XLII", "^[IVXLCDM]*$")
```

To get rid of the namespace prefixes just "use" or open them once at the beginning of your F# file:

```fsharp
open System
open System.Text.RegularExpressions

let r = Int32.TryParse("42")
let m = Regex.Match("XLII", "^[IVXLCDM]*$")
```

## Tuples
You might have noticed unlike F# functions the BCL functions need parentheses around their parameters. That's because, well, they are not F# functions, but prefab functions from an object-oriented .NET library.

C# does not sport partial function application. And the BCL libraries have to be language agnostic. That's why from the F# point of view all BCL functions take just one parameter: a tuple.

A tuple is two or more comma separated values bound together by parentheses:

```fsharp
let p = ("Peter", 34)
let b = ("Beginning F#", "Robert Pickering", 452, 23.21)
```

Tuples are structured values with a fixed number of unnamed heterogeneous elements, i.e. of different type. Lists and arrays are structured values, too. But their unnamed elements all need to be of the same type. And lists have no fixed length.

For now think of tuples as some kind of poor man's records ;-) But look how cheap they are. No type definition needed, no need to use a special type like _Tuple<T0, T1, ...>_ in C#.

Whereas elements in lists and arrays can be access via an index, that's not possible for tuples. Instead you need to deconstruct a tuple using a let binding to get at its fields:

```fsharp
let (name, age) = p
let (title, _, _, price) = b
```

You have to provide as many identifiers on the left side of the = as the tuple on the right side has fields. But if you're not interested in a field's value use an underscore as a "don't care" identifier.

With tuples it's easy to return more than one value from a function, e.g.

```fsharp
let intdiv a b =
  let quotient = a / b
  let remainder = a % b
  quotient, remainder
  
let (q, r) = intdiv 14 4
printfn "%d, %d" q r
```

Or take _TryParse()_ as another example. If used in C# it returns a boolean value to signify success/failure, but it also takes an _out_ parameter for the parsing result. This is [cumbersome](http://luketopia.net/2014/02/05/fsharp-and-output-parameters/), but in C# it would be worse to define its own _struct_ return type or the like.

F# makes this easier. You can consume BCL/C# functions with _out_ parameters as if they returned a tuple:

```fsharp
open System

let (success, value) = Int32.TryParse("42")
printfn "%b, %d" success value
```

Also tuples are the way to go when you want to circumvent partial application. Think of a function  on a 2D point. For such a function you'd want to force its clients to always provide x and y together when calling it. That's accomplished with a tuple:

```fsharp
let translate (x, y) d =
  (x + d, y + d)

let a = translate (2, 5) 3
```

A partial application of _translate_ is still possible - but not with regard to _x_ and _y_. Either both are provided or none of them.

Back to BCL functions. They require you to pass all parameters at once using a tuple, e.g.

```fsharp
open System.Text.RegularExpressions

let m = Regex.Match ("XLII", "^[IVXLCDM]*$")
printfn "%b" m.Success

let months = System.String.Join (",", [|"Jan"; "Feb"; "Mar"|])
printfn "%s" months
```

Or at least it looks like it. Because if you really used a tuple for their parameters it would not work:

```fsharp
let p =  "XLII", "^[IVXLCDM]*$"
let m = Regex.Match p // does not compile!
```

Bummer :-(

But anyway... Calls to BCL or C# functions in an assembly are straightforward to do. Which is also true for accessing properties of objects returned from such functions. See the _Match_ object returned from the _Regex.Match()_ function: To check if a pattern match occurred just query the _Success_ property.

## Refining the solution
With tuples today's task - to implement the function for checking the type of number passed to the conversion program - becomes quite easy. There are two solutions to choose from.

```
open System

let is_roman_number n =
  let (success, _) = System.Int32.TryParse(n)
  not success
```
and

```
open System.Text.RegularExpressions

let is_roman_number n =
  Regex.Match(n, "^[IVXLCDM]*$").Success
```

Which one to choose?

Using the regular expression is slightly smaller, and does not need a negation, which makes it easier to understand. Also using regular expressions is always a demonstration of proficiency, isn't it? ;-)

See [here](src/convertroman) for the current source files of the project.

***

Four of six features implemented! Great! And being able to tap into the power of the .NET BCL sure will turn out to be very useful in the future as well. Maybe already tomorrow...