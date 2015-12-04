[← Day #6 – Recursion](../day06) | [Day #8 – Arrays →](../day08)

# Day #7 – Lists
As you saw yesterday, it's possible to iterate over a collection of values using recursion. That's nice to know – but in many cases it's overkill. Why not write down the values directly as a collection, at least as long as it's not too many?

This is what list literals are about. A list is a non-primitive type; it's a collection of values of the same type listed in a certain order. And it has its own literal syntax: write the list elements separated by ";" between square brackets. Some examples:

```fsharp
let positive = [1; 2; 3]
let abc = ["a"; "b"; "c"]
let ops = [(fun x y -> x + y); (fun a b -> a * b); (-)]
let nested = [[1; 2; 3]; [1; 4; 9]]
```

You see, any value can become an element of a list, even functions and lists themselves.

An empty list is denoted by `[]`.

To access a specific list item you can use the `Item(int)` member function:

```fsharp
printfn "%s" (abc.Item(1))
printfn "%d" (ops.Item(0) 2 3)
```

Or you can do it a bit more array-like, by suffixing the list name with `.[index]`:

```fsharp
printfn "%s" abc.[1]
printfn "%d" (ops.[0] 2 3)
```

Don't be fooled, though. F# lists are not your usual lists you know from C#, for example. F# lists are immutable! You cannot change their content. There is no assignment to an item, there is no deletion of items etc.

They are not even optimized for random access via an index. So even though you can access individual elements like above, you usually want to find a different way.

Working with lists means creating new lists all the time, if you want to apply any changes.

The two most important operators for that are cons (`::`) and concatenate (`@`).

Use cons to prepend a new head to a list, e.g.

```fsharp
let positiveAnd0 = 0 :: positive
let identChars = "_" :: "@" :: abc
let names = "Peter" :: []
```

Use concatenation to append one list to the end of another:

```fsharp
let natural = [-3; -2; -1] @ positiveAnd0
let powers = [1; 10; 100] @ [1; 2; 4; 8] @ [1; 16; 256; 4096]
```

Note that lists are not sets. That means that the value 1 occurs three times in list `powers`. You can check this by printing the list using `%A` as the placeholder:

```fsharp
printfn "%A" powers
```

See how nicely F# formats the output of this non-primitive type?

## Iteration
Now you have lists. But how do you work with them, how do you work with their elements? Yesterday's `iter` function was able to generate the next element by doing a calculation. But with arbitrary and existing lists, how can you move through their items?

Lists provide two additional properties: `Head` and `Tail`.

`Head` returns the first item of a list. This prints 1:

```fsharp
printfn "%d" ([1; 2; 3].Head)
```

`Tail` returns all items after the first as a list. This prints `[2; 3]`:

```fsharp
printfn "%A" ([1; 2; 3]].Tail)
```

With these properties in hand you can create, for example, a `map` function to apply some function to all elements of a list:

```fsharp
let rec map f source =
  if List.isEmpty source
  then []
  else f source.Head :: map f source.Tail
```

`map` applies the function `f` to the head of the list `source` passed in, thus creating a new head for the resulting list. The tail of the resulting list is then created by calling itself recursively with the tail of the `source` list.

If `map` is called with an empty list, it immediately returns an empty list.

```fsharp
map (fun x -> x * x) positive
```

The list of squared values thus does not exist explicitly, but only on the stack while the recursion goes on towards the end of the original `source` list.

This is how working on lists in F# typically looks like – if you need to implement it yourself. Fortunately, though, you don't have to do that all the time. F# comes with quite an assortment of built-in list functions. They are available from the `List` module:

```fsharp
List.map (fun x -> x * x) [1; 2; 3]
List.filter (fun x -> x % 2 = 0) [1; 2; 3]
List.fold (fun s x -> s + x) 0 [1; 2; 3]
List.iter (fun x -> printfn "%d" x) [1; 2; 3]
```

`List.map`, `List.filter`, and `List.fold` are quite useful in many situations as well as easy to understand, especially if you're familiar with .NET LINQ.

* `List.map` corresponds to LINQ `Select()`. The above example thus results in `[1; 4; 9]`.
* `List.filter` corresponds to LINQ `Where()`. The above example returns `[2]`.
* `List.fold` corresponds to LINQ `Aggregate()`. The above example returns `6` as the sum of all elements.
* `List.iter` corresponds to LINQ `ForEach()` for `IList<T>`. The above example prints the elements of the list.

See the [F# documentation](https://msdn.microsoft.com/en-us/library/dd233224.aspx) for more of these functions.

Interestingly, the `List` module also sports `List.head` and `List.tail`. So which should you use? The more functional `List.tail abc` or the more object-oriented `abc.Tail`? The recommendation is: go the functional way! It makes it easier for the compiler to weave its type inference magic.

## Filling lists with a range
So far, lists were initialized with a static literal. That's OK for small lists. But what if a list was to contain dozens, even hundreds of items?

If those items belong to a range then it's easy to fill the list. Just apply the range operator:

```fsharp
let positive = [1..3]
let abc = ['a'..'c']
```

In list brackets, write the lower bound of the range followed by `..` and then the upper bound of the range. F# will create all items in that range for you. It can accomplish this feat for numbers and characters.

And if you like, it can even apply an increment you supply:

```fsharp
let even = [2..2..10]
```

results in `[2; 4; 6; 8; 10]`.

## FizzBuzz Ver 1.0
Back to the "FizzBuzz" kata. How can lists help to make the code even simpler?

First of all, the self-defined function `iter` is not necessary anymore. Instead `List.iter` should be used.

Furthermore, the range of numbers does not need to be produced algorithmically. Instead, a list auto-filled with the numbers can be used.

Finally, it seems a small refactoring is in order. Why not make number checking local to the `fizzbuzz` function?

```fsharp
let fizzbuzz numbers =
  let fizzbuzz_number n =
    let ``is fizz?`` n = n % 3 = 0
    let ``is buzz?`` n = n % 5 = 0
    let ``is fizzbuzz?`` n = (``is fizz?`` n) && (``is buzz?`` n)

    if ``is fizzbuzz?`` n then
      "FizzBuzz"
    else if ``is fizz?`` n then
      "Fizz"
    else if ``is buzz?`` n then
      "Buzz"
    else
      sprintf "%d" n  

  let convert_number i =
    let v = fizzbuzz_number i
    printfn "%s" v

  List.iter convert_number numbers

fizzbuzz [1..20]
```

Speaking of refactoring... As it is, `fizzbuzz` has two responsibilities: mapping numbers and printing the resulting list. The code would be cleaner if these aspects were separated. Maybe like this:

```fsharp
let fizzbuzz numbers =
  let fizzbuzz_number n =
    let ``is fizz?`` n = n % 3 = 0
    let ``is buzz?`` n = n % 5 = 0
    let ``is fizzbuzz?`` n = (``is fizz?`` n) && (``is buzz?`` n)

    if ``is fizzbuzz?`` n then
      "FizzBuzz"
    else if ``is fizz?`` n then
      "Fizz"
    else if ``is buzz?`` n then
      "Buzz"
    else
      sprintf "%d" n  

  List.map fizzbuzz_number numbers

let converted = fizzbuzz [1..20]
List.iter (printfn "%s") converted
```

Now `fizzbuzz` is focused on conversion. What happens to the converted numbers is an altogether different matter.

***

Lists are a very important data structure in F# programs. They'll come in handy throughout the next days when solving more problems.

[← Day #6 – Recursion](../day06) | [Day #8 – Arrays →](../day08)
