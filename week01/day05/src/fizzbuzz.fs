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