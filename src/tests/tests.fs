module tests

open System
open NUnit.Framework

exception MyError
exception InvalidPosition of int * int

// exception InvalidPerson of {name:string; age:int}

type Person = {name:string; age:int}
exception InvalidPerson' of Person

let xyz x = try x + x finally printfn "hello"
let abc x = x + x

[<TestFixture>]
type Tests() = 
   
    [<Test>]
    member x.Test() =
        try
            raise (System.ArgumentException("Wrong!", "name"))
        with
        | :? System.NotImplementedException -> printfn "Not implemented"
        | :? System.ArgumentException as x -> printfn "arg wrong: %s" x.ParamName


        let x = new System.NotImplementedException()
        if x :? System.NotImplementedException then
            printfn "!!!!"
        else
            printfn "????"

        let y = new System.IO.StreamReader("tests.dll")
        if y :? System.IO.StreamReader then printfn "!!" else printfn "??"

        let text = try
                        System.IO.File.ReadAllText("xxxx.dll")
                   with
                   | x -> printfn "*** %A" x
                          ""
        try
            raise (InvalidPosition (-99, 42))
        with
        | InvalidPosition (x,y) -> printfn "%d / %d" x y

        let try_parse_int text =
            try
                Some(System.Int32.Parse(text))
            with
            | _ -> None



        try
            let text = try
                            System.IO.File.AppendAllText("tmp.txt", "x\n")
                            System.IO.File.AppendAllText("tmp.txt", "y\n")
                            failwith "arghh"
                            "hello"
                       finally
                            System.IO.File.Delete("tmp.txt")
            printfn "%s" text
        with
        | x -> printfn "%A" x

        let f : unit = printfn "helloxxx"
        f

        printfn "%A" abc
                       
        printfn "%A" (try_parse_int "42")

        Assert.AreEqual("", text)