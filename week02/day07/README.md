# Day #7 - Lists
As you saw yesterday it's possible to describe a collection of values using recursion. That's nice to know - but in many cases it's overkill. Why not write down the values directly as a collection, at least as long as it's not too many?

This is what list literals are about. A list is a non-primitive type; it's a collection of values of the same type listed in a certain order. And it has its own literal syntax: write the list elements separated by ";" between square brackets. Some examples:

```fsharp
let positive = [1; 2; 3]
let abc = ["a"; "b"; "c"]
let ops = [(fun x y -> x + y); (fun a b -> a * b); (-) ]
let nested = [[1; 2; 3]; [1; 4; 9]]
```

You see, any value can become an element of a list, even functions and lists themselves.

An empty list is denoted by [].

To access a specific list item use the _Item()_ member function:

```fsharp
printfn "%s" (abc.Item(1))
printfn "%d" (ops.Item(0) 2 3)
```

Don't be fooled, though. F# lists are not your usual lists you know from C# for example. F# lists are immutable! You cannot change their content. There is no assignment to an item, there is no deletion of items etc.

Working with lists means creating new lists all the time, if you want to apply any changes.

The two most important operators for that are cons (::) and concatenate (@).

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

Note that lists are not sets. That means the value 1 occurrs three times in list _powers_. You can check this by printing the list using %A as the placeholder:

```fsharp
printfn "%A" powers
```

See, how nicely F# formats the output of this non-primitive type?

## Iteration
Now you have lists. But how to work with them, how to work with their elements? Yesterday's _iter_ function was able to generate the next element by doing a calculation. But with arbitrary and existing lists, how can you move through their items?

Lists provide two additional properties: _Head_ and _Tail_.

_Head_ returns the first item of a list. This prints 1:

```fsharp
printfn "%d" ([1; 2; 3].Head)
``` 

_Tail_ returns all items after the first as a list. This prints [2; 3]:

```fsharp
printfn "%A" ([1; 2; 3]].Tail)
```

With these properties in hand you can create, for example, a function _map_ to apply some function to all elements of a list creating a new list:

```fsharp
let map f source =
  let rec map' f source dest =
    if source = [] then
      dest
    else
      let mv = f (source.Head)
      map' f source.Tail (dest @ [mv])
      
  map' f source []
```

Map, well, maps elements of the source list to elements of a destination list. Example: Map integers to their squares:

```fsharp
map (fun x -> x * x) positive
```

The outer function _map_ is for convenience sake. You can focus on the function and on the list. The inner function _map'_ is the workhorse. It recursively takes the head from the source list, applies the transformation function _f_ and appends the result to the destination list.

If the source has been emptied the destination list is returned as the result.

Note how the concatenation requires the new element to be appended to the destination list to be put in square brackets. The @ operator only works on list. So the single value has to be wrapped up in one.

When calling itself, _map'_ passes on the mapping function, the remaining source list elements, and the newly built destination list.

This is how working on lists in F# typically looks like - if you need to implement it yourself. Fortunately, though, you don't have to all the time. F# comes with quite an assortment of built in list functions. They are available from the _List_ module like this:

```fsharp
List.map (fun x -> x * x) [1; 2; 3]
List.filter (fun x -> x % 2 = 0) [1; 2; 3]
List.fold (fun s x -> s + x) 0 [1; 2; 3]
List.iter (fun x -> printfn "%d" x) [1; 2; 3]
```

_List.map_, _List.filter_, and _List.fold_ are quite useful in many situations as well as easy to understand - especially if you're familiar with .NET Linq.

* _List.map_ corresponds to Linq _Select()_. The above example thus results in [1; 4; 9].
* _List.filter_ corresponds to Linq _Where()_. The above example returns [2].
* _List.fold_ corresponds to Linq _Aggregate()_. The above example returns 6 as the sum of all elements.
* _List.iter_ corresponds to Linq _ForEach()_ for _IList<T>_. The above example prints the elements of the list.

The the [F# documentation](https://msdn.microsoft.com/en-us/library/dd233224.aspx) for more of these functions.

## Filling lists with a range
So far lists were initialized with a static literal. That's ok for small lists. But what if a list was to contain dozens, even hundreds of items?

If those items belong to a range then it's easy to fill the list. Just apply the range operator:

```fsharp
let positive = [1..3]
let abc = ['a'..'c']
```

In list brackets write the lower bound of the range followed by .. and then the upper bound of the range. F# will create all items in that range for you. It can accomplish this feat for numbers and characters.

And if you like it can even apply an increment you supply:

```fsharp
let even = [2..2..10]
```

results in [2; 4; 6; 8; 10]

## FizzBuzz Ver 1.0
Back to the "FizzBuzz" kata. How can lists help to make the code even simpler?

First the self-defined function _iter_ is not necessary anymore. Instead _List.iter_ should be used.

Then the range of numbers does not need to be produced algorithmically. Instead a list auto-filled with the numbers can be used.

Finally it seems a small refactoring is in order. Why not make number checking local to the _fizzbuzz_ function?

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

Speaking of refactoring... As it seems _fizzbuzz_ has two responsibilities: mapping numbers and printing the resulting list. The code would be cleaner if these aspects were separated. Maybe like this:

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

Now _fizzbuzz_ is focused on conversion. What happens to the converted numbers is an altogether different matter.

***

Lists are a very important data structure in F# programs. They'll come in handy throughout the next days when solving more problems.

