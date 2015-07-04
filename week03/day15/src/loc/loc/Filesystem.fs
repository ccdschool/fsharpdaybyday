module Filesystem

let find_source_files (locations:string list) =
    let sourcefiles filename =
        System.IO.Path.GetExtension(filename) = ".cs"

    let from_files =
        let existing_files filename =
            System.IO.File.Exists(filename)

        locations |> List.filter existing_files 
                  |> List.filter sourcefiles

    let from_folders =
        let existing_folders foldername =
            System.IO.Directory.Exists(foldername)
        let to_files foldername =
            System.IO.Directory.GetFiles(foldername, "*.*", System.IO.SearchOption.AllDirectories) |> Array.toList

        locations |> List.filter existing_folders 
                  |> List.map to_files |> List.concat 
                  |> List.filter sourcefiles

    from_files @ from_folders


let compile_source_lines (filenames:string List) =
    let lines = 
        let to_lines filename =
            System.IO.File.ReadAllLines(filename) |> Array.toList

        filenames |> List.map to_lines |> List.concat

    (filenames.Length, lines)