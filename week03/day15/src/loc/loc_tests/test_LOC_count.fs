module test_LOC_count

open System
open NUnit.Framework

[<Test>]
let ``no comments``() =
    let result = LOC.count ["1"; "2"; "3"]
    Assert.AreEqual(3, result)

[<Test>]
let ``single line comment w/o leading whitespace``() =
    let result = LOC.count ["1"; "//-"; "3"]
    Assert.AreEqual(2, result)

[<Test>]
let ``single line comment with leading whitespace``() =
    let result = LOC.count ["1"; " //-"; "\t//-"; "4"]
    Assert.AreEqual(2, result)