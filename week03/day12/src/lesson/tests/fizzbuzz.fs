namespace tests
open System
open NUnit.Framework

[<TestFixture>]
type Fizzbuzz_tests() = 
    let is_fizz i = i % 3 = 0
    let is_buzz i = i % 5 = 0
    let is_fizzbuzz i = (is_fizz i) && (is_buzz i)

    let fizzbuzz i =
        match i with
        | x when is_fizzbuzz x -> "fizzbuzz"
        | x when is_fizz x -> "fizz"
        | x when is_buzz x -> "buzz"
        | _ -> i.ToString()

    let fizzbuzz' = function
        | x when is_fizzbuzz x -> "fizzbuzz"
        | x when is_fizz x -> "fizz"
        | x when is_buzz x -> "buzz"
        | x -> x.ToString()


    let fizzbuzz''' i =
        match i with
        | x when x % 3 = 0 && x % 5 = 0 -> "fizzbuzz"
        | x when x % 3 = 0 -> "fizz"
        | x when x % 5 = 0 -> "buzz"
        | x -> x.ToString()

    let fizzbuzz'' i =
        if i % 3 = 0 && i % 5 = 0 then
            "fizzbuzz"
        else if i % 3 = 0 then
            "fizz"
        else if i % 5 = 0 then
            "buzz"
        else
            i.ToString()

    [<TestCase(1, "1")>]
    [<TestCase(2, "2")>]
    [<TestCase(3, "fizz")>]
    [<TestCase(4, "4")>]
    [<TestCase(5, "buzz")>]
    [<TestCase(6, "fizz")>]
    [<TestCase(15, "fizzbuzz")>]
    member x.Tests(i:int, e:string) =
        Assert.AreEqual(e, fizzbuzz''' i)