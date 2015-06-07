# Day #6 - Recursion
I'm sure you're keen to finish the ["FizzBuzz" code kata](https://app.box.com/s/kvrd51oykrob44xv2t379k98ay9ai568). So let's think about how to do iterations.

In C# or Java or Ruby you'd use some kind of loop to fizzbuzz all the numbers in the range from 1 to 100. Here's an example in C#:

```
for(var i=1; i<=100; i++) {
  var v = Fizzbuzz_number(i);
  Console.WriteLine(v)
}
```

This can be done in F#, too. But there is a more natural way, a more function oriented way of doing it. Maybe you remember from some computer science class that any loop can be converted into a recursion - and any recursion can be converted into a loop.

That's what we want to apply today: We have a looping problem, so let's solve it with recurion.

You already know how to call a function. So you can do recursion, too. Almost at least. Here's the canonical recursion example: a factorial function. But it won't work, yet.

```
let factorial n =
  if n <= 0 then
    1
  else
    n * (factorial (n-1)) // fails to compile!
```

The reason: You need to mark functions to be called recursively as such with _rec_ right after the _let_ keyword.

```
let rec factorial n =
  if n <= 0 then
    1
  else
    n * (factorial (n-1))

printfn "%d" (factorial 6)
```

This now works and prints 720. Great!

But how can this help with iteration over a range of numbers? Well, isn't this a problem, where you do one thing for a value at the head of a list, remove the value from the list - and then repeat this with the remaining elements in the list?

The list in this case is defined as a range, e.g. from 1 until 100. 1 is the head of the list. So something is done with 1, then 1 is removed leaving a list from 2 until 100. Then the same is done with this remaining list - until the list is empty.

That probably sounds more complicated than it is, though. Better to see it in code. Here is a simple iteration function:

```
let rec iter from until =
  if from < until then
    printfn "%d" from
    iter (from + 1) until
  else
    printfn "%d" from
    
iter 0 5
```

Note that abortion of the recursion again is not possible with a just simple _if_ without an _else_ at the beginning of the function. Both the _then_ and the _else_ branch are necessary.

You see the pattern? Recursion is bounded by a condition. If the condition is met, recursion is aborted, otherwise the function calls itself. In each case, whatever should be done with the current value is done.

So why not generalize this pattern? Iterations are all the same whether for "FizzBuzz" or other problems like calculating the squares in a range.

Here is a general iteration function for integer ranges:

```
let rec iter f from until =
  if from < until then
    f from
    iter f (from + 1) until
  else
    f from
    
iter (printfn "%d") 0 5
```

What should be done for each number is passed in as a function! That's true Functional Programming. Remember: functions are values. They can be used like integer or strings. So why not have formal parameters expecting a function to be passed in?

Parameters _from_ and _until_ are used like numbers, so numbers are given when calling _iter_. But _f_ is used like a function, so a function is given when calling _iter_.

_(printfn "%d")_ is a partial application of _printfn_. That's why it's ok to leave out any value to print when using it as a parameter. The missing value later is passed in when _f_ (as an alias name for or value of _printfn "%d"_) is called within _iter_: _f from_. This results in _printfn "%d" from_ being evaluated.

Please note the above general _iter_ function does not return a result, but _unit_ like _printf_ does. Hence you need to pass in a function to also return _unit_. That's the case in the "FizzBuzz" solution.

How to do iterations with functions returning results of other types is the topic for another day.

## FizzBuzz Ver 0.9
With such an iteration function in hand it's easy to finish the "FizzBuzz" solution.

The problem isn't iteration any more. That's solved. Rather the question is: What should be done for each number in the range?

It should be converted and printed.

The final _fizzbuzz_ function hides this detail, of course. It defines the action for each number as a local function and iterates over a given range using _iter_.


```
let fizzbuzz from until =
  let convert_number i =
    let v = fizzbuzz_number i
    printfn "%s" v
  
  iter convert_number from until
    
    
fizzbuzz 1 100
```

See [here](src/fizzbuzz.fs) for the full source code.

***

Recursion and functions as parameters: What a day! That's two powerful techniques of Functional Programming. Still, though, it seems iteration over a range of numbers should be easier than that, right? More on that tomorrow.