module test_Filesystem_find_source_files

open System
open NUnit.Framework

open Adapters

[<Test>]
let ``one filename``() =
    let result = Filesystem.find_source_files ["sampledata/source1.cs"]
    Assert.AreEqual(["sampledata/source1.cs"], result)

[<Test>]
let ``several filenames``() =
    let result = Filesystem.find_source_files ["sampledata/source1.cs"; "sampledata/source2.cs"]
    Assert.AreEqual(["sampledata/source1.cs"; "sampledata/source2.cs"], result)

[<Test>]
let ``skip non-source filenames``() =
    let result = Filesystem.find_source_files ["sampledata/nosource.txt"; "sampledata/source2.cs"]
    Assert.AreEqual(["sampledata/source2.cs"], result)

[<Test>]
let ``skip non-existing files``() =
    let result = Filesystem.find_source_files ["xyz.cs"; "sampledata/source2.cs"]
    Assert.AreEqual(["sampledata/source2.cs"], result)

[<Test>]
let ``files in subdir``() =
    let result = Filesystem.find_source_files ["sampledata/sub/subsub"]
    Assert.AreEqual(["sampledata/sub/subsub/source4.cs"], result)

[<Test>]
let ``files in nested subdirs``() =
    let result = Filesystem.find_source_files ["sampledata/sub"]
    Assert.AreEqual(["sampledata/sub/source3.cs"; "sampledata/sub/subsub/source4.cs"], result)
