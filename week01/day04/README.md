# Day #4 – Naming values
Today is an important day. You'll learn to gain control over your F# code. Because you can control whatever you know the name of. At least that's what fairy tales and lores of magic tell us; remember [Rumpelstiltskin](http://www.eastoftheweb.com/short-stories/UBooks/Rum.shtml)? ;-)

Today we'll start naming things in F# programs. These are just values without a name:

```fsharp
42
"Hello!"
fun x y -> x + y
```

You can combine them

```fsharp
41 + 1
"Hello, " + "World!"
(fun x y -> x + y) 41 1
```

but if you needed a value more than once, you'd have to repeat it. That's against the venerable DRY (Don't Repeat Yourself) principle.

So why not give names to values, to be able to refer to them in a symbolic manner? This is done with so called _let bindings_.

```fsharp
let answer = 42
let greeting = "Hello!"
let add = fun x y -> x + y
```

With _let_, you bind a name to a value. It's like a label you stick on it.

But it's not a variable definition like you might think! Names are just that: names. They are not bins to stuff a value in and later replace the value with a different one.

It's not so much that you assign a value to a name; rather it's assigning a name to a value. And once you've done that, you can use the name instead of the value:

```fsharp
let greeting = "Hello"
let who = "World"
printfn "%s" (greeting + ", " + who + "!")

let almostTheAnswer = 41
let add = fun x y -> x + y
printfn "the answer: %d" (add almostTheAnswer 1)
```

Names make it so much easier to use all those values swirling around in your programs. They are like handles attached to the data. And they add meaning.

By giving a name to a value, like this

```fsharp
let answer = 42
let calculate_volume = fun x y z -> x * y * z
```

you inform the reader about your intention. No need to become an "archaeologist" and ask yourself "What could 42 possibly mean?", "What is fun x y z -> x * y * z for?" Instead, the reader just looks at the name and knows right away. Hopefully ;-). If it's an [intention-revealing name](http://c2.com/cgi/wiki?IntentionRevealingNames) that is.

From this, it should be obvious that a name is singular. It denotes only a single value. You cannot change what a name is referring to. A let binding is immutable.

```fsharp
let a = 41
let a = 42 // does not compile!
```

To make intention revelation even easier, F# gives you quite some liberty in building names. In addition to the usual naming conventions (e.g. start an identifier with a letter or an underscore which can be followed by letters and digits), you can include special characters and even spaces in names. Just enclose them in two backticks:

```fsharp
let ``the ultimate answer!`` = 42
let ``is this the ultimate answer?`` = fun a -> a = ``the ultimate answer!``

printfn "%b" (``is this the ultimate answer?`` 23)
```

So far, names have been given to literal values. But of course you can give them to values resulting from transformations, too.

```fsharp
let greeting = "Hello"
let who = "World"
let welcomeMessage = greeting + ", " + who + "!"

let almostTheAnswer = 41
let theAnswer = almostTheAnswer + 1
```

And that enables you to nest the usage of names:

```fsharp
let build_welcome_message = fun greeting name -> greeting + ", " + name + "!"
let welcome = fun name -> printfn "%s" (build_welcome_message "Hello" name)

welcome "Maria"
```

Names identify "things". A name thus only makes sense if the thing it should denote already exists. Likewise, a name can only be used after it has been assigned to a value. That means F# requires you to always define names before you use them.

```fsharp
let answer = almostTheAnswer + 1 // does not compile!
let almostTheAnswer = 42
```

Finally, since functions are at the core of F# programs, there is some syntactic sugar for defining them.

```fsharp
let add = fun x y -> x + y
```

is the same as

```fsharp
let add x y = x + y
```

You don't need to put a full function literal to the right of the `=` of a let binding. Just write the function parameters behind the identifier and assign both to the function body.

***

Your F# code is gaining meaning. That's good. You're on your way to useful programs. Tomorrow, we're going to solve our first real problem.

PS: To be precise, you can re-bind names to other values, but only within functions. Re-binding does not work outside any function:

```
let a = 41
let a = 42 // does not compile
```

But inside a function you can re-bind a name to even a value of a different type than before:

```
let doSomething x =
    let a = 41
    ...
    let a = "x"
    ...
```

This does not compromise F#'s type safety!
