module Filesystem

let find_source_files (locations:string list) =
    let sourcefiles filename =
        System.IO.Path.GetExtension(filename) = ".cs"
    let existing_files filename =
        System.IO.File.Exists(filename)

    locations |> List.filter sourcefiles |> List.filter existing_files


let compile_source_lines (filenames:string seq) =
    (3, seq ["1"; "//"; "2 //"; "  //"])