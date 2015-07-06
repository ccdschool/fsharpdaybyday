namespace Adapters

module Filesystem =
    let internal find_source_files (locations:Datamodel.Locations) : string list =
        let files_in_folder foldername =
            System.IO.Directory.GetFiles(foldername, "*.*", System.IO.SearchOption.AllDirectories) |> Array.toList

        let (|FileLocation|FolderLocation|InvalidLocation|) location =
            if System.IO.File.Exists(location) then FileLocation(location)
            else if System.IO.Directory.Exists(location) then FolderLocation(location)
            else InvalidLocation

        let gather_files = function
            | FileLocation(filename) -> [filename]
            | FolderLocation(foldername) -> files_in_folder foldername
            | InvalidLocation -> []

        let relevant_files filename =
            System.IO.Path.GetExtension(filename) = ".cs"

        locations |> List.map gather_files |> List.concat |> List.filter relevant_files


    let internal compile_source_lines (filenames:string List) : Datamodel.SourceLines =
        let lines = 
            let to_lines filename =
                System.IO.File.ReadAllLines(filename) |> Array.toList

            filenames |> List.map to_lines |> List.concat

        (filenames.Length, lines)


    let acquire_source_lines =
        find_source_files >> compile_source_lines