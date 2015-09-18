# Day #14 - Exceptions
So much can go wrong at runtime. What can you do to mitigate the effects? If the error is signaled by an exception, then you can catch the exception and log it. F# offers a `try...with` expression for that similar to `try...catch` in C#.

## Catching exceptions
Here's how you can "safely" read a file for example:

```fsharp
try
	System.IO.File.ReadAllText("xyz.txt")
with
| x -> printfn "*** %A" x
       ""
```

`try...with` is an expression, i.e. it returns a value in all cases. If reading from the file succeeds, the file's content is the result. If it fails, an empty string is returned.

And upon failure the exception is printed to the console.

Note the `""` at the end? Since `try...with` is a multi-branch expression like `if...then...else` it needs to return a value of the same type from all branches.

You'll surely recognize `try...with` to be similar to `match...with`. But instead of comparing the value between `try...with` to some patterns, it's the exception that gets compared.

To just get at the exception, do like above. But if you want to react differently depending on the type of exception, then you need to add patterns after `with`, e.g.

```fsharp
try
    ...
with
| :? System.NotImplementedException -> ...
| :? System.ArgumentException as x -> 
		printfn "Invalid parameter: %s" x.ParamName
		...
```

The `:?` operator is for comparison with object types (e.g. `System.IO.StreamReader`, but not `System.DateTime`). You can use it in pattern matching like above or as an infix operator:

```fsharp
if s :? System.IO.StreamReader then ... else ...
```

Other than with `match...with` the list of patterns does not need to be exhaustive. Exceptions not matched simply will bubble up the exception handling stack.

Remember the `TryParse()` method? What if it did not exist and there just was a `Parse()`? With `try...with` you could easily build your own. Just convert the exception into an Option type value:

```fsharp
let try_parse_int text =
    try
        Some(System.Int32.Parse(text))
    with
    | _ -> None
```

## Signal an exceptional situation
If your logic encounters an exceptional situation it might want to signal it to its callers with an exception. That's easily done like this:

```fsharp
if System.IO.File.Exists(filename) then
    ...
else
    failwith "File not found!"
```

`failwith` raises a `System.Exception`. And if you want the message to be formatted, use `failwithf` like `printf`:

```fsharp
failwithf "File not found: %s" filename
```

For more specific exceptions, use `raise` with any exception type. No `new` required for the exception class!

```fsharp
raise (System.ArgumentException("Value must be in range 0..99.", "length"))
```

If you want to raise one of the more "popular" exceptions, though, there are built in functions:

```fsharp
// ArgumentException
invalidArg "length" "Value must be in range 0..99"

// NullArgumentException
nullArg "filename" "Must not be null"

// InvalidOperationException
invalidOp "Calculation not possible w/o values"
```

## Roll your own exception
When there is a domain specific exceptional case to report, you may not want to map it to one of the standard .NET exception classes. Rather you want to throw your own exception. You can define it very simply like this:

```fsharp
exception MyError

exception InvalidPosition of int * int
```

An exception is a special type definition starting with `exception` followed by the exception's name and optionally a parameter type. In this case that's a tuple for `InvalidPosition`.

This means you could define a exception like this:

```fsharp
exception InvalidPerson of {name:string; age:int} // won't compile
```

At least in principle. In practice it's deprecated. Instead define an explicit "payload type":

```fsharp
type Person = {name:string; age:int}
exception InvalidPerson' of Person
```

Self-defined exception types derive from `System.Exception` but do not follow the convention of giving them the suffix "Exception".

You then throw and catch your own exceptions as usual:

```fsharp
try
	...
   raise (InvalidPosition (-99, 42))
with
| InvalidPosition (x,y) -> 
	printfn "Invalid position: %d,%d" x y
	...
```

Note, however, that you don't need the type comparison operator in patterns with your own exception types!

## Clean up
In F# you either catch exceptions with `try...with` _or_ you clean up after an exception `try...finally`.

```fsharp
try
    let text = try
                    System.IO.File.AppendAllText("tmp.txt", ...)
                    ...
                    System.IO.File.AppendAllText("tmp.txt", ...)
                    ...
               finally
                    System.IO.File.Delete("tmp.txt")
    ...
with
| x -> printfn "%A" x
```

As you can see, `try..with` and `try...finally` can be nested - but not combined in one expression. View it as an application of the Single Responsibility Principle.

Other than a `with` clause, `finally` should not return a value but `unit`, the "no value" value of F#.

`1 + 1` returns an `int` value, but for example `printf` returns `unit` or "nothing".

`unit` is a value, but it's a special one signaling "no value". Where in C# `void f(...)` defines a procedure not returning anything, in F# a definition like

```fsharp
let f : unit = ...
```

still creates a function, but one which returns "nothing".

## Using resources
`finally` is executed in all case as usual. But there's one situation you don't need it for: closing an `IDisposable` resource.

In C# you make sure `IDisposable.Dispose()` gets called by wrapping usage of a resource object in a `using` statement:

```csharp
using (var sw = new StreamWriter ("xxx.txt")) {
	sw.WriteLine ("hello");
}
```

F# does not require a special construct for that. Rather it relies on the scope of a binding. If a binding for an `IDisposable` resource goes out of scope `Dispose()` is called automatically - if you bind the object with `use` instead of `let`.

```fsharp
let write (text:string) =
    use sw = new System.IO.StreamWriter("...")
    sw.WriteLine(text)
```

***

Do you now finally feel prepared for real world programming in F# with exceptions under your belt? ;-) Great. So let's tackle another application kata tomorrow...

### Read more
Exceptions

* Microsoft, [Exception Handling (F#)](https://msdn.microsoft.com/en-us/library/dd233223.aspx)
* Scott Wlaschin, [Exceptions - Syntax for throwing and catching](http://fsharpforfunandprofit.com/posts/exceptions/)
* Wikibooks, [F Sharp Programming / Exception Handling](https://en.wikibooks.org/wiki/F_Sharp_Programming/Exception_Handling)

Returning "nothing" from functions

* Microsoft, [Unit Type (F#)](https://msdn.microsoft.com/en-us/library/dd483472.aspx)

Releasing resources

* Microsoft, [Resource Management: The use Keyword (F#)](https://msdn.microsoft.com/en-us/library/dd233240.aspx)