[← Day #11 – Type aliases and record types](../day11) | [Day #13 – Option type →](../day13)

# Day #12 - Pattern matching
Today you'll learn about one of the most important features of any Functional Programming language: pattern matching.

You can start out using pattern matching as a replacement for the `if-then-else` function. Or you can view it as a sophisticated `switch`. But it's much more. It's more like a built in rules engine.

Pattern matching is used in several places in F#. But the most prominent one is the `match` expression.

### Value matching
`if-then-else` is some sort of very simple pattern matching.

```fsharp
if answer=42 then 
    printfn "Question unknown!"
else 
    printfn "You know the question"
```

It checks whether a value matches one of the value patterns `true` or `false`. The same can be done with `match`:

```fsharp
match answer=42 with
| true -> printfn "Question unknown!"
| false -> printfn "You know the question"
```

The general form of a `match` expression is

```fsharp
match someValue with
| pattern1 -> expression1
| pattern2 -> expression2
...
```

`match` compares a given value to a list of patterns which match the type of the value. Above the value's type is `bool` so the patterns are of type `bool`, too.

Think of the vertical bar as an or-operator. This becomes even more obvious when you see that patterns can be written one after another, if they should be handled by the same expression:

```fsharp
match someValue with
| pattern1 | pattern1a | pattern1b -> expression1
| pattern2 | pattern2a | pattern2b -> expression2
...
```

If a pattern matches a value its expression gets evaluated. Of course all expressions must return the same type of data.

Patterns can be constant values like `true` and `false`. _And the list of patterns needs to be exhaustive!_ That means: all possible values need to be covered by at least one pattern. For a given value of type `bool` that's only `true` and `false`. But what, if `match` was to compare patterns to just `answer`?

```fsharp
match answer with
| 42 -> printfn "Question unknown!"
| _ -> printfn "You know the question"
```

In this case the `_` placeholder is a catch all pattern. Which means you need to be careful about the order in which you list the patterns:

```fsharp
match answer with // does not compile
| _ -> printfn "You know the question"
| 42 -> printfn "Question unknown!"
```

This is syntactically correct - but not semantically. The compiler will reject it, since the obvious catch all pattern would block further patterns from being checked. 

_So be sure to order the patterns from specific to general._

But `match` is more than the `switch` you know from other languages. `match` does not only work for scalar types. You can use it with collection types and structured types, too.

You can check for a certain tuple:

```fsharp
let p = "Mary", 42

match p with
| "Peter", 23 -> ...
| "Mary", 42 -> ...
| _ -> ...
```

Or you can check for a particular record:

```fsharp
type Person = {Name:string; Age:int}

let p = {Name="Peter"; Age=23}

match p with
| {Name="Peter"; Age=23} -> ...
| {Name="Mary"; Age=42} -> ...
| _ -> ...
```

Or select a list according to its element values:

```fsharp
let l = [2; 4; 6]

match l with
| [] -> ... // check for empty list
| [2; 4; 6] -> ...
| [1; 3; 5] -> ...
| _ -> ...
```

### Structure matching
This is pretty nifty already, but `match` can do even more. So far you've seen value patterns: a given value is compared to other values, the patterns.

Often, though, the pieces of a structure or collection value are not known (or are not important). Rather you're interested in its structure. Take a list for example. How do you decompose a list? You can do it like this:

```fsharp
let head = l.Head
let tail = l.Tail
```

But what do you do if the list is empty? It's important to take that into account and check for it. There won't be a head in an empty list. That's why the compiler issues a warning when you try to do it like this:

```fsharp
let head :: tail = l // compiler warning issued!
```

This is a simple form of pattern matching, too. Like decomposition of a tuple is:

```fsharp
let p = ("Mary", 42)
let name, age = p
```

The structure on the left side of the `=` must match the structure of the value on the right side.

And this does not only work in let bindings but also with function parameters:

```fsharp
let print_person (name, age) =
    printfn "%s is %d years old" name age
    
print_person p
```

So how to decompose a list safely? `match` to the rescue:

```fsharp
match l with
| [] -> ...
| head :: tail -> printfn "%d - %A" head tail
```

Instead of making the pattern an exact value you just describe the structure you're looking for by using placeholders.

Those placeholders then are bound to the values found in the given value in the places they represent. You can use them on the right side of the `->` operator.

Here's how you can react to persons with certain characteristics using placeholders:

```fsharp
match p with
| {Name="Peter"; Age=a} -> printfn "Peter is %d years old" a
| {Name=n; Age=42} -> printfn "Name of 42 year old is %s" n
| _ -> printfn "Not interested"
```

Or try to group the elements in a list in a pairwise manner, i.e. `[2; 4; 6]` would lead to `[(2,4); (6;6)]`. Here's how to do it with `match`:

```fsharp
let even = [2; 4; 6]

let pair l =
    let rec pair' r l = 
        match l with
        | [] -> r
        | [x] -> (x,x) :: r
        | x :: y :: tail -> pair' ((x,y) :: r) tail 
    pair' [] l |> List.rev
    
printfn "%A" (pair even)
```

Of course the list is processed recursively. On each recursion the remaining list is checked for certain structural patterns: is it empty, or does it only contain a single element, or does it contain at least two elements (plus maybe some more)?

If there are at least two elements in the list they are combined into a tuple and prepended to the resulting list of pairs (parameter `r`).

Prepending `(x,y) :: r` might be a bit more natural than appending `r @ [(x,x)]` since result `r` is already a list; but then the result has to be inverted at the end (`List.rev`). On the other hand appending requires a tad less code - but also has higher runtime costs. You choose ;-)

### Conditional matching
So far pattern matching works based on structure and exact values. But what if the cases should be distinguished where the structure is the same, but the values differ? Take for example the "FizzBuzz" kata again.

Using `if-then-else` the comparison looks like this:

```fsharp
let fizzbuzz i =
    if i % 3 = 0 && i % 5 = 0 then
        "fizzbuzz"
    else if i % 3 = 0 then
        "fizz"
    else if i % 5 = 0 then
        "buzz"
    else
        i.ToString()
```

For each case there is a special expression.

This can be done with `match` too using so called _guarded matches_ with `when`:

```fsharp
let fizzbuzz i =
    match i with
    | x when x % 3 = 0 && x % 5 = 0 -> "fizzbuzz"
    | x when x % 3 = 0 -> "fizz"
    | x when x % 5 = 0 -> "buzz"
    | x -> x.ToString()

```

Do a structure match and then add a conditional expression after `when` to work with the pattern placeholders.

Of course this can be done in an even more readable manner:

```fsharp
let is_fizz i = i % 3 = 0
let is_buzz i = i % 5 = 0
let is_fizzbuzz i = (is_fizz i) && (is_buzz i)

let fizzbuzz i =
    match i with
    | x when is_fizzbuzz x -> "fizzbuzz"
    | x when is_fizz x -> "fizz"
    | x when is_buzz x -> "buzz"
    | x -> x.ToString()
```

With guarded or conditional matches `match` truly becomes the decision making workhorse feature of F#. Once you get the hang of it you'll find you'll use `if-then-else` less and less. It will become more of just a ternary operator used in expressions, e.g.

```fsharp
let line' = line + (if line = "" then "" else ",") + word
```

### Shortcut
Since `match` is used so frequently it should come as no surprise that there is a shortcut for it. If you find yourself defining a function just for the purpose of doing a match like `fizzbuzz` above, then you can shorten it like this:

```fsharp
let fizzbuzz = function
    | x when is_fizzbuzz x -> "fizzbuzz"
    | x when is_fizz x -> "fizz"
    | x when is_buzz x -> "buzz"
    | x -> x.ToString()
```

You can drop the single function parameter as well as `match ... with`. Instead write `function` after the `=` of a let binding followed by the patterns.

This kind of match-function can be used wherever a function value (lambda expression) is allowed, e.g.

```fsharp
[(1,2);(2,2);...] |> List.filter (function x,y when x=y -> true | _ -> false)
```

As you can see, even the vertical bar before the first pattern can be dropped.

This also works for functions with more than one parameter like `pair'` above. Just be sure to pass in the value to match as the last parameter:

```
let pair l =
    let rec pair' r = function
        | [] -> r
        | [x] -> (x,x) :: r
        | x :: y :: tail -> pair' ((x,y) :: r) tail 
    pair' [] l |> List.rev
```

***

Pattern matching is one of the core features of functional languages. At first it might look a bit strange with all the syntax details - but soon you'll get the hang of it.

Just think of it as a function switchboard. Structural patterns almost look like anonymous functions. Compare for example

```
match a with
| x, y -> ...
...
```

to

```
fun x, y -> ... 
```

### Read more
* developerFusion, [Pattern Matching in F# Part 1: Out of the Box](http://www.developerfusion.com/article/132340/pattern-matching-in-f-part-1-out-of-the-box/)
* Microsoft, [Pattern Matching (F#)](https://msdn.microsoft.com/en-us/library/dd547125.aspx)
* Microsoft, [Match Expressions (F#)](https://msdn.microsoft.com/en-us/library/dd233242.aspx)
* Scott Wlaschin, [Match expressions - The workhorse of F#](http://fsharpforfunandprofit.com/posts/match-expression/)
* Wikibooks, [F Sharp Programming / Pattern Matching Basics](https://en.wikibooks.org/wiki/F_Sharp_Programming/Pattern_Matching_Basics)
* Richard Dalton, [Active Patterns](http://www.devjoy.com/series/active-patterns/)

[← Day #11 – Type aliases and record types](../day11) | [Day #13 – Option type →](../day13)
