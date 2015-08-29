# Day #20 - Units of measure
Different types means different kinds of values and possibly different values. This helps to keep operations correct.

So what would you do to ensure, only numbers of the same kind get subtracted or multiplied, e.g. only liters or inches?

Type aliases are too week. But how about union types?

```
type L = L of float
type Inch = Inch of float

let a = L 5.0
let b = Inch 10.0
```

This way numeric values could be more strongly typed and made incompatible on purpose:

```
let c = a + b // does not work
```

Great! But look:

```
let c = a + a // does not work
```

Event though both operands are of the same type the addition does not work. Bummer. The operator is defined for `int` or `float` values, but for some obscure type `L` which only incidentally contains a `float`.

You'd have to overload `+` to make up for that and get a true typesafe addition of `L` values:

```
let (+) (L x) (L y) = L (x + y)

let c = a + a
```

Great again! Type safety achieved.

Or maybe not so great. Because this is a common scenario but it requires quite some effort to make it typesafe.

The common scenario is: Do calculations with numbers of a certain kind. Where _certain kind_ means "with an attached unit of measure".

Wrapping the numbers in a whole new type would be too much. It's still "just" numbers we are talking about. But those numbers come with a little suffix, e.g. 2.5l, 7.3", 10cm, 23.7kg, 0.05sec, 38° etc.

Such units of measure are not so much a matter of mathematics. But in physics and chemistry they are everywhere. And you have to be careful to not combine numbers of different kinds in the wrong way.

What's the right way?

* Adding and subtracting numbers with the same unit of measure is ok.
* Multiplying and dividing numbers with different units of measure is ok, too.

The result of `2m + 3m` is `5m`. But the result of `2m + 10"` is not defined; you first have to convert one of the operands to the unit of measure of the other - if that's possible at all. It works for m and inch, but not for m and kg.

The result of `2m / 10kg`, though, is `0.2m/kg` - whatever that means ;-)

## Defining units of measure
With F# you finally are able to easily make such calculations typesafe where units of measures are involed. F# allows you to define your own units of measures. Just prefix a type with just a name with the attribute `[<Measure>]`:

```
[<Measure>] type l
[<Measure>] type inch
```

Attaching a unit of measure to a `float` or `int` value then is just a matter of suffixing it:

```
let a = 5<l>
let b = 10<inch>
```

This also looks more natural than the above experiment with union types, doesn't it.

As you would expect this prevents values with different units of measures to be added:

```
let c = a + b // does not work
```

But now the addition (and subtraction) of like values is possible:

```
let c = a + a
```

Truely great!

And what about multiplication and division? Works like a charm:

```
let c = a * b
let d = a / b
```

Interestingly this still results in a number with a unit of measure. The type of `c` is `int<inch l>`, and the type of `d` is `int<l/inch>`. F# created new units of measure on the fly.

And now watch:

```
let p = a * a
```

What's the resulting type? `int<l^2>` Isn't that cool? F# does it almost like you'd do on a piece of paper.

This is true even for how you define and use units of measure. Use them like on paper, e.g.

```
let v = 10.0<l/inch>
let v' = 10.0<l inch>
let v'' = 10.0<l^2>
```

And define them as combinations of each other.

```
[<Measure>] type V (* Volt *)
[<Measure>] type A (* Ampere *)
[<Measure>] type Ω = V / A (* Ohm *)
```

Isn't that a dream come true for cheating on physics exercises? ;-)

```
let u = 10.0<V>
let i = 5.0<A>
let r = u / i

let u' = 3.0<Ω> * i
```

And in case you seem to forget Ohm's law you wrap it up in a neat little function:

```
let calc_current (u:float<V>) (r:float<Ω>) : float<A> = 
    u / r
    
let calc_resistance (u:float<V>) (i:float<A>) : float<Ω> = 
    u / i
    
let r = calc_resistance u i
```

Presto! Not only typesafe but "unit-safe" calculations.

## Converting to/from units of measure
Units of measure work fine for the basic math operations like +, -, *, /, . But if you want to do more sophisticated stuff like calculating the sine of a meter-value you're stuck. You need to strip the unit of measure off the value.

That can be done simply by casting the value to a scalar type:

```
let d = 5<m>
...
let d' = float d
let s = System.Math.Sin(d')
```

or you could divide it by 1 with the same unit of measure:

```
let d' = d / 1<m>
```

Which in the case of `Math.Sin()` would still need a conversion to `float`, because `d` was of type `int<m>`. But if it would have been `float<m>` all would have been fine.

And now for the other way around: How to convert a number without a unit of measure to a value with a unit of measure? Maybe you got some data from a file which does not carry the correct unit of measure yet.

Just multiply with 1 and the unit of measure:

```
let rawData = 5
...
let d = rawData * 1<m>
```

Just keep one thing in mind: Units of measure are just a matter of the compiler. You won't find them in compiled code. And don't expect other languages you interoperate with to honor them. They are a F# specific feature.

***

Units of measure are cheap: easy to do, no runtime burden. But they make things so much more clear. Go, use them!

### Read more

Units of measure

* Microsoft, [Units of Measure (F#)](https://msdn.microsoft.com/en-us/library/dd233243.aspx)
* Scott Wlaschin, [Units of measure](http://fsharpforfunandprofit.com/posts/units-of-measure/)
* Wikibooks, [F Sharp Programming / Units of Measure](https://en.wikibooks.org/wiki/F_Sharp_Programming/Units_of_Measure)

