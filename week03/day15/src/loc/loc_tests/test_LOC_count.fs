module test_LOC_count

open System
open NUnit.Framework

open Domain

[<Test>]
let ``no comments``() =
    let result = LOC.count ["1"; "2"; "3"]
    Assert.AreEqual(3, result)

[<Test>]
let ``single line comment w/o leading whitespace``() =
    let result = LOC.count ["1"; "//-"; "2"]
    Assert.AreEqual(2, result)

[<Test>]
let ``single line comment with leading whitespace``() =
    let result = LOC.count ["1"; " //-"; "\t//-"; "2"]
    Assert.AreEqual(2, result)

[<Test>]
let ``multi line comment with leading and trailing whitespae``() =
    let result = LOC.count ["1"; " /*-"; "-"; "-*/ "; "2"]
    Assert.AreEqual(2, result)

[<Test>]
let ``multi line comment in single line``() =
    let result = LOC.count ["1"; " /*-*/ "; "2"]
    Assert.AreEqual(2, result)

[<Test>]
let ``multi line comment with leading and trailing source``() =
    let result = LOC.count ["1"; "2 /*+"; "-"; "+*/ 3"; "4"]
    Assert.AreEqual(4, result)

[<Test>]
let ``whitespace lines``() =
    let result = LOC.count ["1"; ""; " \t"; "2"]
    Assert.AreEqual(2, result)