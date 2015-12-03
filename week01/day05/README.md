[&lt; Day #4 – Naming values](../day04)

# Day #5 – Simple decisions with if
Programming is about data processing, i.e. calculating values and assigning them to variables. As you've seen, the first is true also for F#; but the latter is not really how you do things in functional programming languages. But don't let this worry you too much right now.

As a small consolation, today you'll meet an old acquaintance, a control structure like in any imperative programming language. Meet the _if_ expression:

```fsharp
let name = "Peter"
if name = "Paul" then printfn "Hello, my friend!" else printfn "Hello, stranger!"
```

This looks familiar, doesn't it? Even more if I add some indentation:

```fsharp
let name = "Peter"

if name = "Paul" then
  printfn "Hello, my friend!"
else
  printfn "Hello, stranger!"
```

Depending on the result of the boolean expression, the comparison, either this or that string is written to the console.

But what looks like a statement in truth is still an expression. We're talking functional programming, so view `if` as a function taking three arguments: a boolean value to decide on, what to do on true, and what to do on false.

The result of the `if` function, then, is whatever the result is of the branch that got executed. The next example will make that more clear:

```fsharp
let greeting name =
  if name = "Paul" then
    "Hello, my friend!"
  else
    "Hello, stranger!"

printfn "%s" (greeting "Peter")
```

`greeting` is a function with one parameter. Its definition spans several lines. So far, functions were one-liners. But if you indent the expressions making up the body of a function, you can spread them out over multiple lines. Just be sure to use spaces, not tabs, to indent!

Whatever is indented below the let binding line forms the body of it. Of course, the same indentation level is required for all lines. This helps to let your code look tidy. No more quibbling about such details of code formatting. No more matching curly braces.

Since `if` returns a result, both the `then` and the `else` branches need to be present and deliver a value of the same type. That's why

```fsharp
let div a b =
  if b <> 0 then
    a / b // does not compile!
  else
    "Division by zero!"
```

does not compile. One branch returns an `int`, the other a `string`.

There are ways to deal with this kind of scenario. But that's for another day.

Now let's tackle a real, although small, problem.

## FizzBuzz Ver 0.5
Do you know [code katas](http://en.wikipedia.org/wiki/Kata_(programming)), those small programming challenges to exercise coding skills like [TDD](http://en.wikipedia.org/wiki/Test-driven_development)?

Here's such a kata, almost a hello-world kata for any [coding dojo](http://codingdojo.org). It's called "FizzBuzz".

Read the full description [here](https://app.box.com/s/kvrd51oykrob44xv2t379k98ay9ai568). It takes just two minutes.

You probably already envision a solution to this problem in your regular programming language. But how to do it in F#?

To be frank, in order to solve the whole problem, there is a puzzle piece missing. You haven't seen yet how to do iterations.

Nevertheless, you can tackle the core problem. That is: translate a single number into its "FizzBuzz value". Here's a walking skeleton for a stripped-down version of the program.

```fsharp
let fizzbuzz_number n =
  sprintf "%d" n

printfn "%b" ((fizzbuzz_number 1) = "1")
printfn "%b" ((fizzbuzz_number 3) = "Fizz")
printfn "%b" ((fizzbuzz_number 5) = "Buzz")
printfn "%b" ((fizzbuzz_number 12) = "Fizz")
printfn "%b" ((fizzbuzz_number 15) = "FizzBuzz")
printfn "%b" ((fizzbuzz_number 17) = "17")
```

A first, all too simple function definition with a couple of poor man's automated tests.

Note the `sprintf` function, which prints to a string whereas `printf` prints to the console. It's just a placeholder for the real logic of the solution.

But the real solution is not very complicated either:

```fsharp
let fizzbuzz_number n =
  if n % 3 = 0 && n % 5 = 0 then
    "FizzBuzz"
  else if n % 3 = 0 then
    "Fizz"
  else if n % 5 = 0 then
    "Buzz"
  else
    sprintf "%d" n
```

It's just a couple of nested `if` expressions.

Although this logic serves the purpose, it's arguably not clean, not DRY. Checking for Fizz and Buzz numbers is done in two places.

That can be improved by introducing functions for these checks:

```fsharp
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
```

Since they are of no interest to the outside, they are defined locally within the scope of `fizzbuzz_number`. A feat not possible in many other languages, including C#. But a very useful language feature, because it improves encapsulation by limiting the visibility of functions.

That's it. Problem solved. Or at least the first part of the problem.

And since this was so easy, let's get rid of some code duplication in the testing part of the program.

```fsharp
let assert_result given expected =
  if (fizzbuzz_number given) = expected then
    printfn "OK for %d: %s" given expected
  else
    printfn "FAILED for %d, expected %s" given expected

assert_result 1 "1"
assert_result 3 "Fizz"
assert_result 5 "Buzz"
assert_result 12 "Fizz"
assert_result 15 "FizzBuzz"
assert_result 17 "17"
```

As you see, it's quite easy to come up with your own small automated test framework ;-).

***

You've put F# to real use for the first time. One of the prevalent problems of computing science has been solved. Almost, at least ;-). Let's see if we can finish it tomorrow.

[&lt; Day #4 – Naming values](../day04)
