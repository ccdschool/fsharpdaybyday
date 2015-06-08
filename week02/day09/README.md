# Day #9
Today we'll focus on just one feature of the ["Convert roman" kata](https://app.box.com/s/z07b8gr6e1ngvb3cg7ps78zy2ddi3vx1): determine the type of number to be converted. Is it a roman number or an arabic one?

The function to accomplish this is

```
let is_roman_number n = ...
```

with a type of _string -> bool_.






```
let is_roman_number n =
  let (success, _) = System.Int32.TryParse(n)
  not success
```

```
open System

let is_roman_number n =
  let (success, _) = System.Int32.TryParse(n)
  not success
```


```
open System.Text.RegularExpressions

let is_roman_number n =
  Regex.Match(n, "^[IVXLCDM]*$").Success
```


tuples
calling clr functions
	tryparse
	regex
