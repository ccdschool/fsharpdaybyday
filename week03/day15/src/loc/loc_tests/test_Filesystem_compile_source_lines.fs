module test_Filesystem_compile_source_lines

open System
open NUnit.Framework

open Adapters

[<Test>]
let ``one file``() =
    let n, lines = Filesystem.compile_source_lines ["sampledata/source1.cs"]
    Assert.AreEqual(1, n)
    Assert.AreEqual(7, lines.Length)


[<Test>]
let ``more than one file``() =
    let n, lines = Filesystem.compile_source_lines ["sampledata/source1.cs"; "sampledata/source2.cs"]
    Assert.AreEqual(2, n)
    Assert.AreEqual(17, lines.Length)